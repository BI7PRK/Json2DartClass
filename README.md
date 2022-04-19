# Json2DartClass
代码来自 https://github.com/muhammad369/JsonToDart. 重写的目的是修正了一个类型错误问题。当JOSN里类型为`double`的值为`x.0`时，生成的代码会认为是 `int`类型。并且进行
简化`Serializable`文件。丰富了生成代码。
