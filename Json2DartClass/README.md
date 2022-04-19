## 可以从 ASP.NET 的网站中通过 `JSON` 生成 `Dart Class`文件。
代码来自 [URL(https://github.com/muhammad369/JsonToDart)] 重写是为了纠正一个类型问题。当 double 类型的值为 x.0时，它会认为是int类型。并且去除了`Serializable class`。丰富了生成的代码。