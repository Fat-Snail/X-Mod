## XCode无实体模型使用案例

有时候会遇到一些不需要使用实体模型的场景，譬如单纯的读取某些第三方提供的数据表，不可能每次都要生成相应的实体对数据库进行操作，所以为了适应此场景，无实体操作数据库使用XCode组件也一样便捷。

核心点，为了强调能适配所有数据库，一般情况下不推荐直接编写SQL语句，不然到时候切换数据库的时候，底层逻辑又得适配一遍。那废话不多说，直接上代码

### 目录

- [数据库连接方式](#id1)
  - 1.App.config和appsetting.json方式
  - 2.代码注入
  - 3.进阶技巧
- [简单查询](#id2)
  - 1.查询 
  - 2.新增与修改 
  - 3.删除


***

### 数据库连接方式 <a id="id1"> </a>

#### 1. App.config和appsetting.json方式

这个看官方文档 ## 就好，写得非常详细，所以不细写了，主要代码如下：

```xml

  <connectionStrings>
    <add name="SQLite" connectionString="Data Source=test.db;" providerName="Sqlite" />
    <add name="MySql" connectionString="Server=.;Port=3306;Database=mysql;Uid=root;Pwd=;NameFormat=Upper" providerName="MySql.Data.MySqlClient" />
  </connectionStrings>

  "ConnectionStrings": {
    "SQLite": {
      "connectionString": "Data Source=test.db;",
      "providerNamee": "Sqlite"
    },
    "MySql": {
      "connectionString": "Server=.;Port=3306;Database=mysql;Uid=root;Pwd=;",
      "providerName": "MySql.Data.MySqlClient"
    }
  }
```

#### 2. 代码注入

```csharp
   DAL.AddConnStr("SQLite", "Data Source=test.db;provider=sqlite", null, null);

```
SQLite是ConnName，可以是任意名称，后面会用到

Data Source=test.db;provider=sqlite  这个字符串指定了sqlite的连接和数据库类型，这么写，你的变量是多种数据库连接都行，譬如我想连接SQLServer，就改成：

```csharp
   DAL.AddConnStr("SQLite", "Data Source=.;Initial Catalog=userDB;Trusted_Connection=yes;provider=mssql", null, null);

```

这样就可以通杀所有数据库了，provider类型主要有以下几种：

Sqlserver(注意要小写)
- mssql
- sqlserver
- sqlclient
- system.data.sqlclient

SQLite
- sqlite

MySql
- mysql
- mysql.data.mysqlclient

Oracle
- oracle
- oracleclient

PostgreSQL
- postgresql
- npgsql

其他不常用的就不举例了

已经注入数据库连接就可以创建了
```csharp
    var dal = DAL.Create("SQLite"); //SQLite是可以任意的名字，当然要跟前面一致
```

有了这个dal就可以各种操作数据库了


#### 3. 进阶技巧

譬如我想获取某个表结构

```csharp
var dal = DAL.Create("SQLite");

var tables = dal.Tables;//获取所有表

var createTableSql = dal.GetCreateTableSql(tables.ToArray());//如果表太多慎用

var table = tables.Find(x => x.Name == "user");//获取user表

var userTableSql = dal.GetCreateTableSql(table);//获取user表的创建语句
```

查询表数据

```csharp

    var sb = new SelectBuilder { Table = "User", Where = "UserId=1" };
    var dt = dal.Query(sb);

    //等同于,但是推荐前者，因为对所有数据库是适配的
    dt = dal.Query("Select * From User Where UserId=1");

```
拿到dt之后就可以编写各种导入导出，保存或者其他的逻辑处理了

有些经典的使用案例是，譬如我拿了A库往B库导数据，这样可以使用以下操作：

```csharp

    DAL.AddConnStr("BakDB", "Data Source=User.db;provider=sqlite", null, null);
    DAL.AddConnStr("ToBakDB", "Data Source=.;Initial Catalog=userDB;Trusted_Connection=yes;provider=mssql", null, null);
    var dal = DAL.Create("BakDB");
    var toDal = DAL.Create("ToBakDB");

    var table = dal.Tables.Find(x => x.Name == "User");
    //把自增键去掉，不然会影响效率，作为备份库，自增键也不是很重要
    var col = table.Columns.FirstOrDefault(e => e.Identity);
    if (col != null)
    {
        col.Identity = false;
    }

    var sb = new SelectBuilder { Table = table.Name, Where = "Age>18" };
    var dt = dal.Query(sb);

    toDal.SetTables(table);//先确保导入库有没有创建User表
    //批量导入到Sqlserver数据库
    toDal.Session.Insert(table, table.Columns.ToArray(), dt.Cast<IModel>());

```

### 简单查询 <a id="id2"> </a>

#### 1. 查询

```csharp
    //重点推荐SelectBuilder类，第一兼容性好，第二可以避免打错
    var sb = new SelectBuilder { Table = table.Name, Where = "Age>18" };
    var dt = dal.Query(sb);

    //支持参数化查询
    sb = new SelectBuilder { Table = table.Name, Where = "Age>@age" };
    sb.Parameters = new List<IDataParameter> { dal.Db.CreateParameter("@age", 18) };//这里多多少少有点不太优雅，可以升级一下
    dt = dal.Query(sb);

    //只取五条，记得排序，聪明的小伙伴应该都能不看我的写案例，SelectBuilder已经提供
    sb.OrderBy = " id asc ";
    dt = dal.Query(sb, 0, 5)；

    //这里为了省事，都是来源于官方的测试案例，请忽略使用了实体，把语句都按上面的逻辑改一样可以
    //支持分页
    var list =  dal.Query<User>("select * from user where name=@name", new { Name = "admin" }, new PageParameter { PageIndex = 1, PageSize = 20 }).ToList();


```
#### 2. 新增与修改

```csharp
    //这个有点不符合标题，但是实体的结构是比较清晰的
    var user = new User { Id = Rand.Next(), Name = Rand.NextString(8) };
    dal.Insert(user, "user");

    //这是无实体的操作，其实都是一样的原理
    dal.Insert(new { Id = Rand.Next(), Name = Rand.NextString(8) }, "user");
    //修改
    dal.Update(new { enable = true }, new { id = user.Id }, "user");

```
#### 3. 删除

```csharp
    //就是这么简单
    dal.Delete("user", new { id = user.Id });

```