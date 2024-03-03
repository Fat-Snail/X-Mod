## SyncDB工具

市面上有很多数据库同步的软件，出名的有heidisql。收费的有navicat，但大多数都是实现简单数据拷贝，而且性能一般，因为基本都是单线程。而且批量操作总有那么一丢丢不如意。所以衍生了写这个工具的想法。

- 此项目涉及到开源的组件有：
  - 1.[NewLife.XCode](https://github.com/NewLifeX/NewLife.XCode)
  
  因为有些底层不太兼容，也不知道NewLife.XCode啥时候Merge我的代码，所以出现bug时，可以使用我Fork的源码：https://github.com/Fat-Snail/NewLife.XCode

  - 2.[SunnyUI](https://github.com/yhuse/SunnyUI)

代码涉及到大量无实体操作数据库的操作，如果你要修改此源码，最好看一下
[XCode无实体模型使用案例.md](https://github.com/Fat-Snail/X-Mod/blob/main/XCode%E6%97%A0%E5%AE%9E%E4%BD%93%E6%A8%A1%E5%9E%8B%E4%BD%BF%E7%94%A8%E6%A1%88%E4%BE%8B.md)



本工具理论上支持所有数据库的复制及同步，暂时测试以下几种同步案例：

- Sqlte To Sqlite
- Sqlte To MySql
  
  注意：如果操作MySql数据库失败时，请用nuget引用MySql.Data.6.7.9 的包，更高版本可能会依赖许多dll，6.7是比较干净且不影响操作的版本
- Sqlte To SQLServer
  
  注意要引用 System.Data.SqlClient

其实就是已测试Sqlte、 MySql、SQLServer这三种数据库的同步方案，其他方案因为xcode底层支持，所以理论上应该没问题

后期会出兼容所有系统的的UI，如果你想完善此功能，也可以加入我们


Enjoy～  ^_^

## 新生命开发团队
![XCode](https://newlifex.com/logo.png)  

新生命团队（NewLife）成立于2002年，是新时代物联网行业解决方案提供者，致力于提供软硬件应用方案咨询、系统架构规划与开发服务。  
团队主导的70多个开源项目已被广泛应用于各行业，Nuget累计下载量高达100余万次。  
团队开发的大数据中间件NewLife.XCode、蚂蚁调度计算平台AntJob、星尘分布式平台Stardust、缓存队列组件NewLife.Redis以及物联网平台FIoT，均成功应用于电力、高校、互联网、电信、交通、物流、工控、医疗、文博等行业，为客户提供了大量先进、可靠、安全、高质量、易扩展的产品和系统集成服务。  

我们将不断通过服务的持续改进，成为客户长期信赖的合作伙伴，通过不断的创新和发展，成为国内优秀的IoT服务供应商。  

`新生命团队始于2002年，部分开源项目具有20年以上漫长历史，源码库保留有2010年以来所有修改记录`  
网站：https://newlifex.com  
开源：https://github.com/newlifex  
QQ群：1600800/1600838  
微信公众号：  
![智能大石头](https://newlifex.com/stone.jpg)