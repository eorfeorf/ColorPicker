# ColorPicker
A simple color picker.

![image](https://user-images.githubusercontent.com/10098082/181616510-0a47dcf5-32dd-4af0-be97-43d3e2664009.png)



# UPM Package
1. Window > PackageManager > + > Add package from git URL...
2. You add "https://github.com/eorfeorf/ColorPicker.git?path=Assets/ColorPicker/#v1.2.1" to Package Manager.

# Preparation
Install [UniRx v7.1.0](https://github.com/neuecc/UniRx/releases/tag/7.1.0) and TextMeshPro(v3.0.6).
TextMeshPro is an official Unity package.

# How to use
You put "Package/SimpleColorPicker/Runtime/Prefabs/ColorPicker.prefab" in a scene.
Or instantiate "ColorPicker.prefab".

``` example.cs
// 変更中.
colorPicker.OnChanged.Subscribe(changedColor =>
{
    image.color = changedColor;
}).AddTo(this);

// セーブボタン.
colorPicker.OnSaveButton.Subscribe(newColor =>
{
    image.color = newColor;
}).AddTo(this);

// キャンセルボタン.
colorPicker.OnCancelButton.Subscribe(nowColor =>
{
    image.color = nowColor;
}).AddTo(this);

// 閉じるボタン.
colorPicker.OnCloseButton.Subscribe(colors =>
{
    // 閉じた時に変更後の色と変更前の色が選べる.
    // image.color = colors.nowColor;
    image.color = colors.newColor;
}).AddTo(this);


// プログラムから開く.
// 引数で変更前の色を設定できる.
colorPicker.Open(Color.white);

// プログラムから閉じる.
colorPicker.Close();
```

# License
This library is under the MIT License.
