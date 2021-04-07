# BIDSSMemLibTester
[![Pack Latest Build](https://github.com/Tralsys/BIDSSMemLibTester/actions/workflows/PackAction.yml/badge.svg)](https://github.com/Tralsys/BIDSSMemLibTester/actions/workflows/PackAction.yml)
[![Build And Test](https://github.com/Tralsys/BIDSSMemLibTester/actions/workflows/TestAction.yml/badge.svg)](https://github.com/Tralsys/BIDSSMemLibTester/actions/workflows/TestAction.yml)

BIDSSmemLibを使用し, BIDSSMemのテストを行うツール群です.

## License
すべてMITライセンスの下で使用できます.

## Testers
### BIDSSMemLibWriteTester
BIDS Shared Memoryに書き込むテストを行うツールです.  .Net5.0がインストールされた環境下で動作します.

構文は, 次の通りです.
~~~
set panel AAA:XXX BBB:YYY
~~~
上記例における`AAA`および`BBB`はインデックス指定文字列を意味します.  また, `XXX`, `YYY`は, それぞれコロンで接続されたインデックス指定文字列に対応するpanelに設定する値を意味します.
上記例にてAAA, BBBとも単純な数値文字列であった場合, `panel[AAA]`の値を`XXX`に, `panel[BBB]`の値を`YYY`に, それぞれ変更する操作を意味します.

インデックス指定文字列は, 単純な数値(例:`12`), あるいは数値2つをアンダーバーで接続した範囲指定用文字列(例:`15_17`), およびそれらをコンマで複数まとめた文字列(例:`20,22_27,30,1_5,9_11`)からなります.  
範囲指定では, アンダーバーの前の数値の場所から, アンダーバーの後の数値の場所のひとつ前までが対象となります.  (例:`10_13`の場合, これを展開すると`10,11,12`になります.)

なお, インデックス指定文字列が意味するインデックスは, 0以上256未満である必要があります.  また, 例えば上記例においてAAAとBBBの指定に重複があった場合, 重複箇所には後の設定値, つまり`YYY`が設定されます.

例として, Panel[10]とPanel[15]に1を, Panel[20]とPanel[30], Panel[31], Panel[32]に2を設定する場合, 次のようなコマンドを実行します.
```
set panel 10,15:1 20,30_33:2
```

なお, `set`および`panel`は, それぞれ`s`や`p`等に省略可能です.
