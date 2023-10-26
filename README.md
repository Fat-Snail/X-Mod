# X-Mod

## 在Mac的环境生成XCode实体
最近要用Mac 写代码，所以需要生成XCode的实体类，发现Mac版的Visual Studio对T4模版支持不太友好，CrazyCoder也不能在Mac平台运行，需要安装GKT+3，所以折腾来折腾去就只剩xcodetool这个工具了。

这是xcodetool的修改版，主要是为了方便提供多个项目生成实体，所以必须满足以下需求

- 工具本身与项目无关，不然一个项目得配一个生成工具
- Model.xml必须跟项目绑定，方便管理
- 必须生成在类库项目里面，不然代码文件得拷来拷去

基于以上想法，xcodetool只是个控制台工具，所以必须得支持命令行传参

第一个参数是 Model.xml的读取路径
另外一个是生成输出类库的文件目录

```
xcodetool -mi <Model.xml PATH> -output <Dir PATH>
```

克隆此项目可自行编译

编译完成后会在上层目录找到 Bin/net6.0 生成的项目文件如下

```
NewLife.Core.dll
XCode.dll
xcodetool
xcodetool.des.json
xcodetool.dll
xcodetool.pdb
xcodetool.runtimeconfig.json
xcodetool.xml
```

你也可以拷贝到你想存放的目录，这些文件不需要任何配置，因为所有的参数都源于命令行的输入


新建一个类库 DataEntity 存放在 /Users/DEV/Projects/DataEntity/

    > 注意：/Users/DEV/Projects/DataEntity/ 是举例路径，可以根据自己实际情况定义

首先在类库根目录下编写 Model.xml 模型文件

然后在类库根目录新建一个 build.command 苹果批处理文件，跟window的bat文件类似，然后编写以下命令行：

```
/Users/DEV/Projects/Bin/net6.0/xcodetool -mi /Users/DEV/Projects/DataEntity/Model.xml -output /Users/DEV/Projects/DataEntity/
```

/Users/DEV/Projects/Bin/net6.0/xcodetool 是我刚才编译的 xcodetool 项目生成的文件

最后给 build.command 文件最高权限，不然双击会报权限不够的错误

打开 终端 大致命令如下：

```
# cd /Users/DEV/Projects/DataEntity/
# ls -l
-rw-r--r--@ 1 DEV  staff   3649 10 26 14:28 Model.xml
-rw-r--r--@ 1 DEV  staff   3649 10 26 14:28 build.command

# chmod +x build.command
# ls -l
-rw-r--r--@ 1 DEV  staff   3649 10 26 14:29 Model.xml
--rwxr-xr-x@ 1 DEV  staff   3649 10 26 14:29 build.command

```

然后双击 build.command 就可以生成实体文件了

尽情的开始 Mac 编程之旅

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