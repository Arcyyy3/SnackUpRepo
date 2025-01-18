; ModuleID = 'marshal_methods.armeabi-v7a.ll'
source_filename = "marshal_methods.armeabi-v7a.ll"
target datalayout = "e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "armv7-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [156 x ptr] zeroinitializer, align 4

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [312 x i32] [
	i32 42639949, ; 0: System.Threading.Thread => 0x28aa24d => 146
	i32 67008169, ; 1: zh-Hant\Microsoft.Maui.Controls.resources => 0x3fe76a9 => 33
	i32 68219467, ; 2: System.Security.Cryptography.Primitives => 0x410f24b => 142
	i32 72070932, ; 3: Microsoft.Maui.Graphics.dll => 0x44bb714 => 53
	i32 117431740, ; 4: System.Runtime.InteropServices => 0x6ffddbc => 137
	i32 149972175, ; 5: System.Security.Cryptography.Primitives.dll => 0x8f064cf => 142
	i32 165246403, ; 6: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 82
	i32 172961045, ; 7: Syncfusion.Maui.Core.dll => 0xa4f2d15 => 67
	i32 179457679, ; 8: Syncfusion.Maui.Expander => 0xab24e8f => 68
	i32 182336117, ; 9: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 101
	i32 195452805, ; 10: vi/Microsoft.Maui.Controls.resources.dll => 0xba65f85 => 30
	i32 199333315, ; 11: zh-HK/Microsoft.Maui.Controls.resources.dll => 0xbe195c3 => 31
	i32 205061960, ; 12: System.ComponentModel => 0xc38ff48 => 116
	i32 209399409, ; 13: Xamarin.AndroidX.Browser.dll => 0xc7b2e71 => 80
	i32 280992041, ; 14: cs/Microsoft.Maui.Controls.resources.dll => 0x10bf9929 => 2
	i32 315399044, ; 15: Syncfusion.Maui.Expander.dll => 0x12cc9b84 => 68
	i32 317674968, ; 16: vi\Microsoft.Maui.Controls.resources => 0x12ef55d8 => 30
	i32 318968648, ; 17: Xamarin.AndroidX.Activity.dll => 0x13031348 => 77
	i32 336156722, ; 18: ja/Microsoft.Maui.Controls.resources.dll => 0x14095832 => 15
	i32 342366114, ; 19: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 89
	i32 356389973, ; 20: it/Microsoft.Maui.Controls.resources.dll => 0x153e1455 => 14
	i32 364942007, ; 21: SkiaSharp.Extended.UI => 0x15c092b7 => 56
	i32 374376850, ; 22: Syncfusion.Maui.Popup.dll => 0x16508992 => 69
	i32 376991480, ; 23: en-US/Syncfusion.Maui.Buttons.resources.dll => 0x16786ef8 => 34
	i32 379916513, ; 24: System.Threading.Thread.dll => 0x16a510e1 => 146
	i32 382590210, ; 25: SkiaSharp.Extended.dll => 0x16cddd02 => 55
	i32 385762202, ; 26: System.Memory.dll => 0x16fe439a => 127
	i32 395744057, ; 27: _Microsoft.Android.Resource.Designer => 0x17969339 => 35
	i32 435591531, ; 28: sv/Microsoft.Maui.Controls.resources.dll => 0x19f6996b => 26
	i32 442565967, ; 29: System.Collections => 0x1a61054f => 113
	i32 450948140, ; 30: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 88
	i32 456227837, ; 31: System.Web.HttpUtility.dll => 0x1b317bfd => 148
	i32 469710990, ; 32: System.dll => 0x1bff388e => 150
	i32 498788369, ; 33: System.ObjectModel => 0x1dbae811 => 134
	i32 500358224, ; 34: id/Microsoft.Maui.Controls.resources.dll => 0x1dd2dc50 => 13
	i32 503918385, ; 35: fi/Microsoft.Maui.Controls.resources.dll => 0x1e092f31 => 7
	i32 504833739, ; 36: SkiaSharp.SceneGraph => 0x1e1726cb => 59
	i32 513247710, ; 37: Microsoft.Extensions.Primitives.dll => 0x1e9789de => 47
	i32 525008092, ; 38: SkiaSharp.dll => 0x1f4afcdc => 54
	i32 539058512, ; 39: Microsoft.Extensions.Logging => 0x20216150 => 44
	i32 568561038, ; 40: Syncfusion.Maui.ProgressBar.dll => 0x21e38d8e => 70
	i32 592146354, ; 41: pt-BR/Microsoft.Maui.Controls.resources.dll => 0x234b6fb2 => 21
	i32 597488923, ; 42: CommunityToolkit.Maui => 0x239cf51b => 37
	i32 613668793, ; 43: System.Security.Cryptography.Algorithms => 0x2493d7b9 => 141
	i32 623678609, ; 44: Syncfusion.Maui.ProgressBar => 0x252c9491 => 70
	i32 627609679, ; 45: Xamarin.AndroidX.CustomView => 0x2568904f => 86
	i32 627931235, ; 46: nl\Microsoft.Maui.Controls.resources => 0x256d7863 => 19
	i32 662205335, ; 47: System.Text.Encodings.Web.dll => 0x27787397 => 75
	i32 672442732, ; 48: System.Collections.Concurrent => 0x2814a96c => 109
	i32 676419328, ; 49: en-US\Syncfusion.Maui.Buttons.resources => 0x28515700 => 34
	i32 688181140, ; 50: ca/Microsoft.Maui.Controls.resources.dll => 0x2904cf94 => 1
	i32 690602616, ; 51: SkiaSharp.Skottie.dll => 0x2929c278 => 60
	i32 695450347, ; 52: Syncfusion.Maui.Popup => 0x2973baeb => 69
	i32 706645707, ; 53: ko/Microsoft.Maui.Controls.resources.dll => 0x2a1e8ecb => 16
	i32 709557578, ; 54: de/Microsoft.Maui.Controls.resources.dll => 0x2a4afd4a => 4
	i32 722857257, ; 55: System.Runtime.Loader.dll => 0x2b15ed29 => 138
	i32 738469988, ; 56: SkiaSharp.SceneGraph.dll => 0x2c042864 => 59
	i32 759454413, ; 57: System.Net.Requests => 0x2d445acd => 132
	i32 775507847, ; 58: System.IO.Compression => 0x2e394f87 => 124
	i32 777317022, ; 59: sk\Microsoft.Maui.Controls.resources => 0x2e54ea9e => 25
	i32 778756650, ; 60: SkiaSharp.HarfBuzz.dll => 0x2e6ae22a => 57
	i32 778804420, ; 61: SkiaSharp.Extended.UI.dll => 0x2e6b9cc4 => 56
	i32 789151979, ; 62: Microsoft.Extensions.Options => 0x2f0980eb => 46
	i32 823281589, ; 63: System.Private.Uri.dll => 0x311247b5 => 135
	i32 830298997, ; 64: System.IO.Compression.Brotli => 0x317d5b75 => 123
	i32 878811702, ; 65: SnackUpClient => 0x34619a36 => 108
	i32 878954865, ; 66: System.Net.Http.Json => 0x3463c971 => 128
	i32 904024072, ; 67: System.ComponentModel.Primitives.dll => 0x35e25008 => 114
	i32 919194361, ; 68: Syncfusion.Maui.Calendar.dll => 0x36c9caf9 => 66
	i32 926902833, ; 69: tr/Microsoft.Maui.Controls.resources.dll => 0x373f6a31 => 28
	i32 966729478, ; 70: Xamarin.Google.Crypto.Tink.Android => 0x399f1f06 => 105
	i32 967690846, ; 71: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 89
	i32 992768348, ; 72: System.Collections.dll => 0x3b2c715c => 113
	i32 1012816738, ; 73: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 99
	i32 1019214401, ; 74: System.Drawing => 0x3cbffa41 => 121
	i32 1028951442, ; 75: Microsoft.Extensions.DependencyInjection.Abstractions => 0x3d548d92 => 43
	i32 1029334545, ; 76: da/Microsoft.Maui.Controls.resources.dll => 0x3d5a6611 => 3
	i32 1030482433, ; 77: SnackUpClient.dll => 0x3d6bea01 => 108
	i32 1035644815, ; 78: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 78
	i32 1036536393, ; 79: System.Drawing.Primitives.dll => 0x3dc84a49 => 120
	i32 1044663988, ; 80: System.Linq.Expressions.dll => 0x3e444eb4 => 125
	i32 1052210849, ; 81: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 91
	i32 1082857460, ; 82: System.ComponentModel.TypeConverter => 0x408b17f4 => 115
	i32 1084122840, ; 83: Xamarin.Kotlin.StdLib => 0x409e66d8 => 106
	i32 1098259244, ; 84: System => 0x41761b2c => 150
	i32 1118262833, ; 85: ko\Microsoft.Maui.Controls.resources => 0x42a75631 => 16
	i32 1126950560, ; 86: Syncfusion.Maui.PullToRefresh.dll => 0x432be6a0 => 71
	i32 1140297880, ; 87: SkiaSharp.Resources.dll => 0x43f79098 => 58
	i32 1168523401, ; 88: pt\Microsoft.Maui.Controls.resources => 0x45a64089 => 22
	i32 1176943841, ; 89: Syncfusion.Maui.TabView => 0x4626bce1 => 72
	i32 1178241025, ; 90: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 96
	i32 1203215381, ; 91: pl/Microsoft.Maui.Controls.resources.dll => 0x47b79c15 => 20
	i32 1208641965, ; 92: System.Diagnostics.Process => 0x480a69ad => 118
	i32 1234928153, ; 93: nb/Microsoft.Maui.Controls.resources.dll => 0x499b8219 => 18
	i32 1260983243, ; 94: cs\Microsoft.Maui.Controls.resources => 0x4b2913cb => 2
	i32 1293217323, ; 95: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 87
	i32 1324164729, ; 96: System.Linq => 0x4eed2679 => 126
	i32 1373134921, ; 97: zh-Hans\Microsoft.Maui.Controls.resources => 0x51d86049 => 32
	i32 1376866003, ; 98: Xamarin.AndroidX.SavedState => 0x52114ed3 => 99
	i32 1406073936, ; 99: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 83
	i32 1430672901, ; 100: ar\Microsoft.Maui.Controls.resources => 0x55465605 => 0
	i32 1435222561, ; 101: Xamarin.Google.Crypto.Tink.Android.dll => 0x558bc221 => 105
	i32 1452070440, ; 102: System.Formats.Asn1.dll => 0x568cd628 => 122
	i32 1461004990, ; 103: es\Microsoft.Maui.Controls.resources => 0x57152abe => 6
	i32 1461234159, ; 104: System.Collections.Immutable.dll => 0x5718a9ef => 110
	i32 1462112819, ; 105: System.IO.Compression.dll => 0x57261233 => 124
	i32 1469204771, ; 106: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 79
	i32 1470490898, ; 107: Microsoft.Extensions.Primitives => 0x57a5e912 => 47
	i32 1479771757, ; 108: System.Collections.Immutable => 0x5833866d => 110
	i32 1480492111, ; 109: System.IO.Compression.Brotli.dll => 0x583e844f => 123
	i32 1493001747, ; 110: hi/Microsoft.Maui.Controls.resources.dll => 0x58fd6613 => 10
	i32 1514721132, ; 111: el/Microsoft.Maui.Controls.resources.dll => 0x5a48cf6c => 5
	i32 1537889881, ; 112: Syncfusion.Maui.Buttons.dll => 0x5baa5659 => 65
	i32 1543031311, ; 113: System.Text.RegularExpressions.dll => 0x5bf8ca0f => 145
	i32 1551623176, ; 114: sk/Microsoft.Maui.Controls.resources.dll => 0x5c7be408 => 25
	i32 1596911864, ; 115: Syncfusion.Maui.Buttons => 0x5f2ef0f8 => 65
	i32 1622152042, ; 116: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 93
	i32 1623212457, ; 117: SkiaSharp.Views.Maui.Controls => 0x60c041a9 => 62
	i32 1624863272, ; 118: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 103
	i32 1634654947, ; 119: CommunityToolkit.Maui.Core.dll => 0x616edae3 => 38
	i32 1636350590, ; 120: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 85
	i32 1639515021, ; 121: System.Net.Http.dll => 0x61b9038d => 129
	i32 1639986890, ; 122: System.Text.RegularExpressions => 0x61c036ca => 145
	i32 1657153582, ; 123: System.Runtime => 0x62c6282e => 140
	i32 1658251792, ; 124: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 104
	i32 1677501392, ; 125: System.Net.Primitives.dll => 0x63fca3d0 => 131
	i32 1679769178, ; 126: System.Security.Cryptography => 0x641f3e5a => 143
	i32 1724472758, ; 127: SkiaSharp.Extended => 0x66c95db6 => 55
	i32 1729485958, ; 128: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 81
	i32 1732996618, ; 129: Syncfusion.Maui.TabView.dll => 0x674b6e0a => 72
	i32 1736233607, ; 130: ro/Microsoft.Maui.Controls.resources.dll => 0x677cd287 => 23
	i32 1743415430, ; 131: ca\Microsoft.Maui.Controls.resources => 0x67ea6886 => 1
	i32 1746115085, ; 132: System.IO.Pipelines.dll => 0x68139a0d => 74
	i32 1763938596, ; 133: System.Diagnostics.TraceSource.dll => 0x69239124 => 119
	i32 1766324549, ; 134: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 101
	i32 1770582343, ; 135: Microsoft.Extensions.Logging.dll => 0x6988f147 => 44
	i32 1780572499, ; 136: Mono.Android.Runtime.dll => 0x6a216153 => 154
	i32 1782862114, ; 137: ms\Microsoft.Maui.Controls.resources => 0x6a445122 => 17
	i32 1788241197, ; 138: Xamarin.AndroidX.Fragment => 0x6a96652d => 88
	i32 1793755602, ; 139: he\Microsoft.Maui.Controls.resources => 0x6aea89d2 => 9
	i32 1808609942, ; 140: Xamarin.AndroidX.Loader => 0x6bcd3296 => 93
	i32 1813058853, ; 141: Xamarin.Kotlin.StdLib.dll => 0x6c111525 => 106
	i32 1813201214, ; 142: Xamarin.Google.Android.Material => 0x6c13413e => 104
	i32 1818569960, ; 143: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 97
	i32 1824175904, ; 144: System.Text.Encoding.Extensions => 0x6cbab720 => 144
	i32 1828688058, ; 145: Microsoft.Extensions.Logging.Abstractions.dll => 0x6cff90ba => 45
	i32 1842015223, ; 146: uk/Microsoft.Maui.Controls.resources.dll => 0x6dcaebf7 => 29
	i32 1853025655, ; 147: sv\Microsoft.Maui.Controls.resources => 0x6e72ed77 => 26
	i32 1858542181, ; 148: System.Linq.Expressions => 0x6ec71a65 => 125
	i32 1875935024, ; 149: fr\Microsoft.Maui.Controls.resources => 0x6fd07f30 => 8
	i32 1910275211, ; 150: System.Collections.NonGeneric.dll => 0x71dc7c8b => 111
	i32 1961813231, ; 151: Xamarin.AndroidX.Security.SecurityCrypto.dll => 0x74eee4ef => 100
	i32 1968388702, ; 152: Microsoft.Extensions.Configuration.dll => 0x75533a5e => 40
	i32 2003115576, ; 153: el\Microsoft.Maui.Controls.resources => 0x77651e38 => 5
	i32 2019465201, ; 154: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 91
	i32 2025202353, ; 155: ar/Microsoft.Maui.Controls.resources.dll => 0x78b622b1 => 0
	i32 2045470958, ; 156: System.Private.Xml => 0x79eb68ee => 136
	i32 2055257422, ; 157: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 90
	i32 2066184531, ; 158: de\Microsoft.Maui.Controls.resources => 0x7b277953 => 4
	i32 2070888862, ; 159: System.Diagnostics.TraceSource => 0x7b6f419e => 119
	i32 2079903147, ; 160: System.Runtime.dll => 0x7bf8cdab => 140
	i32 2090596640, ; 161: System.Numerics.Vectors => 0x7c9bf920 => 133
	i32 2127167465, ; 162: System.Console => 0x7ec9ffe9 => 117
	i32 2142473426, ; 163: System.Collections.Specialized => 0x7fb38cd2 => 112
	i32 2159891885, ; 164: Microsoft.Maui => 0x80bd55ad => 51
	i32 2169148018, ; 165: hu\Microsoft.Maui.Controls.resources => 0x814a9272 => 12
	i32 2181898931, ; 166: Microsoft.Extensions.Options.dll => 0x820d22b3 => 46
	i32 2192057212, ; 167: Microsoft.Extensions.Logging.Abstractions => 0x82a8237c => 45
	i32 2193016926, ; 168: System.ObjectModel.dll => 0x82b6c85e => 134
	i32 2201107256, ; 169: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x83323b38 => 107
	i32 2201231467, ; 170: System.Net.Http => 0x8334206b => 129
	i32 2207618523, ; 171: it\Microsoft.Maui.Controls.resources => 0x839595db => 14
	i32 2266799131, ; 172: Microsoft.Extensions.Configuration.Abstractions => 0x871c9c1b => 41
	i32 2270573516, ; 173: fr/Microsoft.Maui.Controls.resources.dll => 0x875633cc => 8
	i32 2279755925, ; 174: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 98
	i32 2297679138, ; 175: SkiaSharp.Resources => 0x88f3cd22 => 58
	i32 2298471582, ; 176: System.Net.Mail => 0x88ffe49e => 130
	i32 2303942373, ; 177: nb\Microsoft.Maui.Controls.resources => 0x89535ee5 => 18
	i32 2305521784, ; 178: System.Private.CoreLib.dll => 0x896b7878 => 152
	i32 2353062107, ; 179: System.Net.Primitives => 0x8c40e0db => 131
	i32 2354730003, ; 180: Syncfusion.Licensing => 0x8c5a5413 => 64
	i32 2364201794, ; 181: SkiaSharp.Views.Maui.Core => 0x8ceadb42 => 63
	i32 2368005991, ; 182: System.Xml.ReaderWriter.dll => 0x8d24e767 => 149
	i32 2371007202, ; 183: Microsoft.Extensions.Configuration => 0x8d52b2e2 => 40
	i32 2395872292, ; 184: id\Microsoft.Maui.Controls.resources => 0x8ece1c24 => 13
	i32 2401565422, ; 185: System.Web.HttpUtility => 0x8f24faee => 148
	i32 2427813419, ; 186: hi\Microsoft.Maui.Controls.resources => 0x90b57e2b => 10
	i32 2435356389, ; 187: System.Console.dll => 0x912896e5 => 117
	i32 2471841756, ; 188: netstandard.dll => 0x93554fdc => 151
	i32 2475788418, ; 189: Java.Interop.dll => 0x93918882 => 153
	i32 2480646305, ; 190: Microsoft.Maui.Controls => 0x93dba8a1 => 49
	i32 2519222276, ; 191: Syncfusion.Maui.Calendar => 0x96284804 => 66
	i32 2550873716, ; 192: hr\Microsoft.Maui.Controls.resources => 0x980b3e74 => 11
	i32 2570120770, ; 193: System.Text.Encodings.Web => 0x9930ee42 => 75
	i32 2585220780, ; 194: System.Text.Encoding.Extensions.dll => 0x9a1756ac => 144
	i32 2593496499, ; 195: pl\Microsoft.Maui.Controls.resources => 0x9a959db3 => 20
	i32 2605712449, ; 196: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x9b500441 => 107
	i32 2617129537, ; 197: System.Private.Xml.dll => 0x9bfe3a41 => 136
	i32 2620871830, ; 198: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 85
	i32 2625339995, ; 199: SkiaSharp.Views.Maui.Core.dll => 0x9c7b825b => 63
	i32 2626831493, ; 200: ja\Microsoft.Maui.Controls.resources => 0x9c924485 => 15
	i32 2663698177, ; 201: System.Runtime.Loader => 0x9ec4cf01 => 138
	i32 2665622720, ; 202: System.Drawing.Primitives => 0x9ee22cc0 => 120
	i32 2707746672, ; 203: Syncfusion.Maui.PullToRefresh => 0xa164ef70 => 71
	i32 2724373263, ; 204: System.Runtime.Numerics.dll => 0xa262a30f => 139
	i32 2732626843, ; 205: Xamarin.AndroidX.Activity => 0xa2e0939b => 77
	i32 2737747696, ; 206: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 79
	i32 2752995522, ; 207: pt-BR\Microsoft.Maui.Controls.resources => 0xa41760c2 => 21
	i32 2758225723, ; 208: Microsoft.Maui.Controls.Xaml => 0xa4672f3b => 50
	i32 2764765095, ; 209: Microsoft.Maui.dll => 0xa4caf7a7 => 51
	i32 2778768386, ; 210: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 102
	i32 2785988530, ; 211: th\Microsoft.Maui.Controls.resources => 0xa60ecfb2 => 27
	i32 2795602088, ; 212: SkiaSharp.Views.Android.dll => 0xa6a180a8 => 61
	i32 2801831435, ; 213: Microsoft.Maui.Graphics => 0xa7008e0b => 53
	i32 2806116107, ; 214: es/Microsoft.Maui.Controls.resources.dll => 0xa741ef0b => 6
	i32 2810250172, ; 215: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 83
	i32 2831556043, ; 216: nl/Microsoft.Maui.Controls.resources.dll => 0xa8c61dcb => 19
	i32 2853208004, ; 217: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 102
	i32 2861189240, ; 218: Microsoft.Maui.Essentials => 0xaa8a4878 => 52
	i32 2868488919, ; 219: CommunityToolkit.Maui.Core => 0xaaf9aad7 => 38
	i32 2868557005, ; 220: Syncfusion.Licensing.dll => 0xaafab4cd => 64
	i32 2909740682, ; 221: System.Private.CoreLib => 0xad6f1e8a => 152
	i32 2912489636, ; 222: SkiaSharp.Views.Android => 0xad9910a4 => 61
	i32 2916838712, ; 223: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 103
	i32 2919462931, ; 224: System.Numerics.Vectors.dll => 0xae037813 => 133
	i32 2959614098, ; 225: System.ComponentModel.dll => 0xb0682092 => 116
	i32 2970759306, ; 226: BCrypt.Net-Next.dll => 0xb112308a => 36
	i32 2972252294, ; 227: System.Security.Cryptography.Algorithms.dll => 0xb128f886 => 141
	i32 2978675010, ; 228: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 87
	i32 2987532451, ; 229: Xamarin.AndroidX.Security.SecurityCrypto => 0xb21220a3 => 100
	i32 3038032645, ; 230: _Microsoft.Android.Resource.Designer.dll => 0xb514b305 => 35
	i32 3057625584, ; 231: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 94
	i32 3059408633, ; 232: Mono.Android.Runtime => 0xb65adef9 => 154
	i32 3059793426, ; 233: System.ComponentModel.Primitives => 0xb660be12 => 114
	i32 3077302341, ; 234: hu/Microsoft.Maui.Controls.resources.dll => 0xb76be845 => 12
	i32 3103600923, ; 235: System.Formats.Asn1 => 0xb8fd311b => 122
	i32 3147228406, ; 236: Syncfusion.Maui.Core => 0xbb96e4f6 => 67
	i32 3178803400, ; 237: Xamarin.AndroidX.Navigation.Fragment.dll => 0xbd78b0c8 => 95
	i32 3220365878, ; 238: System.Threading => 0xbff2e236 => 147
	i32 3258312781, ; 239: Xamarin.AndroidX.CardView => 0xc235e84d => 81
	i32 3305363605, ; 240: fi\Microsoft.Maui.Controls.resources => 0xc503d895 => 7
	i32 3316684772, ; 241: System.Net.Requests.dll => 0xc5b097e4 => 132
	i32 3317135071, ; 242: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 86
	i32 3340387945, ; 243: SkiaSharp => 0xc71a4669 => 54
	i32 3346324047, ; 244: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 96
	i32 3357674450, ; 245: ru\Microsoft.Maui.Controls.resources => 0xc8220bd2 => 24
	i32 3358260929, ; 246: System.Text.Json => 0xc82afec1 => 76
	i32 3362522851, ; 247: Xamarin.AndroidX.Core => 0xc86c06e3 => 84
	i32 3366347497, ; 248: Java.Interop => 0xc8a662e9 => 153
	i32 3374999561, ; 249: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 98
	i32 3381016424, ; 250: da\Microsoft.Maui.Controls.resources => 0xc9863768 => 3
	i32 3428513518, ; 251: Microsoft.Extensions.DependencyInjection.dll => 0xcc5af6ee => 42
	i32 3430777524, ; 252: netstandard => 0xcc7d82b4 => 151
	i32 3452344032, ; 253: Microsoft.Maui.Controls.Compatibility.dll => 0xcdc696e0 => 48
	i32 3463511458, ; 254: hr/Microsoft.Maui.Controls.resources.dll => 0xce70fda2 => 11
	i32 3471940407, ; 255: System.ComponentModel.TypeConverter.dll => 0xcef19b37 => 115
	i32 3472012038, ; 256: BCrypt.Net-Next => 0xcef2b306 => 36
	i32 3473156932, ; 257: SkiaSharp.Views.Maui.Controls.dll => 0xcf042b44 => 62
	i32 3476120550, ; 258: Mono.Android => 0xcf3163e6 => 155
	i32 3479583265, ; 259: ru/Microsoft.Maui.Controls.resources.dll => 0xcf663a21 => 24
	i32 3484440000, ; 260: ro\Microsoft.Maui.Controls.resources => 0xcfb055c0 => 23
	i32 3485117614, ; 261: System.Text.Json.dll => 0xcfbaacae => 76
	i32 3580758918, ; 262: zh-HK\Microsoft.Maui.Controls.resources => 0xd56e0b86 => 31
	i32 3608519521, ; 263: System.Linq.dll => 0xd715a361 => 126
	i32 3641597786, ; 264: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 90
	i32 3643446276, ; 265: tr\Microsoft.Maui.Controls.resources => 0xd92a9404 => 28
	i32 3643854240, ; 266: Xamarin.AndroidX.Navigation.Fragment => 0xd930cda0 => 95
	i32 3657292374, ; 267: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd9fdda56 => 41
	i32 3663323240, ; 268: SkiaSharp.Skottie => 0xda59e068 => 60
	i32 3672681054, ; 269: Mono.Android.dll => 0xdae8aa5e => 155
	i32 3682565725, ; 270: Xamarin.AndroidX.Browser => 0xdb7f7e5d => 80
	i32 3697841164, ; 271: zh-Hant/Microsoft.Maui.Controls.resources.dll => 0xdc68940c => 33
	i32 3724971120, ; 272: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 94
	i32 3737834244, ; 273: System.Net.Http.Json.dll => 0xdecad304 => 128
	i32 3748608112, ; 274: System.Diagnostics.DiagnosticSource => 0xdf6f3870 => 73
	i32 3786282454, ; 275: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 82
	i32 3792276235, ; 276: System.Collections.NonGeneric => 0xe2098b0b => 111
	i32 3792835768, ; 277: HarfBuzzSharp => 0xe21214b8 => 39
	i32 3800979733, ; 278: Microsoft.Maui.Controls.Compatibility => 0xe28e5915 => 48
	i32 3802395368, ; 279: System.Collections.Specialized.dll => 0xe2a3f2e8 => 112
	i32 3817368567, ; 280: CommunityToolkit.Maui.dll => 0xe3886bf7 => 37
	i32 3823082795, ; 281: System.Security.Cryptography.dll => 0xe3df9d2b => 143
	i32 3841636137, ; 282: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xe4fab729 => 43
	i32 3844307129, ; 283: System.Net.Mail.dll => 0xe52378b9 => 130
	i32 3849253459, ; 284: System.Runtime.InteropServices.dll => 0xe56ef253 => 137
	i32 3889960447, ; 285: zh-Hans/Microsoft.Maui.Controls.resources.dll => 0xe7dc15ff => 32
	i32 3896106733, ; 286: System.Collections.Concurrent.dll => 0xe839deed => 109
	i32 3896760992, ; 287: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 84
	i32 3928044579, ; 288: System.Xml.ReaderWriter => 0xea213423 => 149
	i32 3931092270, ; 289: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 97
	i32 3955647286, ; 290: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 78
	i32 3980434154, ; 291: th/Microsoft.Maui.Controls.resources.dll => 0xed409aea => 27
	i32 3987592930, ; 292: he/Microsoft.Maui.Controls.resources.dll => 0xedadd6e2 => 9
	i32 4003436829, ; 293: System.Diagnostics.Process.dll => 0xee9f991d => 118
	i32 4003906742, ; 294: HarfBuzzSharp.dll => 0xeea6c4b6 => 39
	i32 4023392905, ; 295: System.IO.Pipelines => 0xefd01a89 => 74
	i32 4025784931, ; 296: System.Memory => 0xeff49a63 => 127
	i32 4046471985, ; 297: Microsoft.Maui.Controls.Xaml.dll => 0xf1304331 => 50
	i32 4066802364, ; 298: SkiaSharp.HarfBuzz => 0xf2667abc => 57
	i32 4073602200, ; 299: System.Threading.dll => 0xf2ce3c98 => 147
	i32 4094352644, ; 300: Microsoft.Maui.Essentials.dll => 0xf40add04 => 52
	i32 4099507663, ; 301: System.Drawing.dll => 0xf45985cf => 121
	i32 4100113165, ; 302: System.Private.Uri => 0xf462c30d => 135
	i32 4102112229, ; 303: pt/Microsoft.Maui.Controls.resources.dll => 0xf48143e5 => 22
	i32 4125707920, ; 304: ms/Microsoft.Maui.Controls.resources.dll => 0xf5e94e90 => 17
	i32 4126470640, ; 305: Microsoft.Extensions.DependencyInjection => 0xf5f4f1f0 => 42
	i32 4150914736, ; 306: uk\Microsoft.Maui.Controls.resources => 0xf769eeb0 => 29
	i32 4182413190, ; 307: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 92
	i32 4213026141, ; 308: System.Diagnostics.DiagnosticSource.dll => 0xfb1dad5d => 73
	i32 4271975918, ; 309: Microsoft.Maui.Controls.dll => 0xfea12dee => 49
	i32 4274976490, ; 310: System.Runtime.Numerics => 0xfecef6ea => 139
	i32 4292120959 ; 311: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 92
], align 4

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [312 x i32] [
	i32 146, ; 0
	i32 33, ; 1
	i32 142, ; 2
	i32 53, ; 3
	i32 137, ; 4
	i32 142, ; 5
	i32 82, ; 6
	i32 67, ; 7
	i32 68, ; 8
	i32 101, ; 9
	i32 30, ; 10
	i32 31, ; 11
	i32 116, ; 12
	i32 80, ; 13
	i32 2, ; 14
	i32 68, ; 15
	i32 30, ; 16
	i32 77, ; 17
	i32 15, ; 18
	i32 89, ; 19
	i32 14, ; 20
	i32 56, ; 21
	i32 69, ; 22
	i32 34, ; 23
	i32 146, ; 24
	i32 55, ; 25
	i32 127, ; 26
	i32 35, ; 27
	i32 26, ; 28
	i32 113, ; 29
	i32 88, ; 30
	i32 148, ; 31
	i32 150, ; 32
	i32 134, ; 33
	i32 13, ; 34
	i32 7, ; 35
	i32 59, ; 36
	i32 47, ; 37
	i32 54, ; 38
	i32 44, ; 39
	i32 70, ; 40
	i32 21, ; 41
	i32 37, ; 42
	i32 141, ; 43
	i32 70, ; 44
	i32 86, ; 45
	i32 19, ; 46
	i32 75, ; 47
	i32 109, ; 48
	i32 34, ; 49
	i32 1, ; 50
	i32 60, ; 51
	i32 69, ; 52
	i32 16, ; 53
	i32 4, ; 54
	i32 138, ; 55
	i32 59, ; 56
	i32 132, ; 57
	i32 124, ; 58
	i32 25, ; 59
	i32 57, ; 60
	i32 56, ; 61
	i32 46, ; 62
	i32 135, ; 63
	i32 123, ; 64
	i32 108, ; 65
	i32 128, ; 66
	i32 114, ; 67
	i32 66, ; 68
	i32 28, ; 69
	i32 105, ; 70
	i32 89, ; 71
	i32 113, ; 72
	i32 99, ; 73
	i32 121, ; 74
	i32 43, ; 75
	i32 3, ; 76
	i32 108, ; 77
	i32 78, ; 78
	i32 120, ; 79
	i32 125, ; 80
	i32 91, ; 81
	i32 115, ; 82
	i32 106, ; 83
	i32 150, ; 84
	i32 16, ; 85
	i32 71, ; 86
	i32 58, ; 87
	i32 22, ; 88
	i32 72, ; 89
	i32 96, ; 90
	i32 20, ; 91
	i32 118, ; 92
	i32 18, ; 93
	i32 2, ; 94
	i32 87, ; 95
	i32 126, ; 96
	i32 32, ; 97
	i32 99, ; 98
	i32 83, ; 99
	i32 0, ; 100
	i32 105, ; 101
	i32 122, ; 102
	i32 6, ; 103
	i32 110, ; 104
	i32 124, ; 105
	i32 79, ; 106
	i32 47, ; 107
	i32 110, ; 108
	i32 123, ; 109
	i32 10, ; 110
	i32 5, ; 111
	i32 65, ; 112
	i32 145, ; 113
	i32 25, ; 114
	i32 65, ; 115
	i32 93, ; 116
	i32 62, ; 117
	i32 103, ; 118
	i32 38, ; 119
	i32 85, ; 120
	i32 129, ; 121
	i32 145, ; 122
	i32 140, ; 123
	i32 104, ; 124
	i32 131, ; 125
	i32 143, ; 126
	i32 55, ; 127
	i32 81, ; 128
	i32 72, ; 129
	i32 23, ; 130
	i32 1, ; 131
	i32 74, ; 132
	i32 119, ; 133
	i32 101, ; 134
	i32 44, ; 135
	i32 154, ; 136
	i32 17, ; 137
	i32 88, ; 138
	i32 9, ; 139
	i32 93, ; 140
	i32 106, ; 141
	i32 104, ; 142
	i32 97, ; 143
	i32 144, ; 144
	i32 45, ; 145
	i32 29, ; 146
	i32 26, ; 147
	i32 125, ; 148
	i32 8, ; 149
	i32 111, ; 150
	i32 100, ; 151
	i32 40, ; 152
	i32 5, ; 153
	i32 91, ; 154
	i32 0, ; 155
	i32 136, ; 156
	i32 90, ; 157
	i32 4, ; 158
	i32 119, ; 159
	i32 140, ; 160
	i32 133, ; 161
	i32 117, ; 162
	i32 112, ; 163
	i32 51, ; 164
	i32 12, ; 165
	i32 46, ; 166
	i32 45, ; 167
	i32 134, ; 168
	i32 107, ; 169
	i32 129, ; 170
	i32 14, ; 171
	i32 41, ; 172
	i32 8, ; 173
	i32 98, ; 174
	i32 58, ; 175
	i32 130, ; 176
	i32 18, ; 177
	i32 152, ; 178
	i32 131, ; 179
	i32 64, ; 180
	i32 63, ; 181
	i32 149, ; 182
	i32 40, ; 183
	i32 13, ; 184
	i32 148, ; 185
	i32 10, ; 186
	i32 117, ; 187
	i32 151, ; 188
	i32 153, ; 189
	i32 49, ; 190
	i32 66, ; 191
	i32 11, ; 192
	i32 75, ; 193
	i32 144, ; 194
	i32 20, ; 195
	i32 107, ; 196
	i32 136, ; 197
	i32 85, ; 198
	i32 63, ; 199
	i32 15, ; 200
	i32 138, ; 201
	i32 120, ; 202
	i32 71, ; 203
	i32 139, ; 204
	i32 77, ; 205
	i32 79, ; 206
	i32 21, ; 207
	i32 50, ; 208
	i32 51, ; 209
	i32 102, ; 210
	i32 27, ; 211
	i32 61, ; 212
	i32 53, ; 213
	i32 6, ; 214
	i32 83, ; 215
	i32 19, ; 216
	i32 102, ; 217
	i32 52, ; 218
	i32 38, ; 219
	i32 64, ; 220
	i32 152, ; 221
	i32 61, ; 222
	i32 103, ; 223
	i32 133, ; 224
	i32 116, ; 225
	i32 36, ; 226
	i32 141, ; 227
	i32 87, ; 228
	i32 100, ; 229
	i32 35, ; 230
	i32 94, ; 231
	i32 154, ; 232
	i32 114, ; 233
	i32 12, ; 234
	i32 122, ; 235
	i32 67, ; 236
	i32 95, ; 237
	i32 147, ; 238
	i32 81, ; 239
	i32 7, ; 240
	i32 132, ; 241
	i32 86, ; 242
	i32 54, ; 243
	i32 96, ; 244
	i32 24, ; 245
	i32 76, ; 246
	i32 84, ; 247
	i32 153, ; 248
	i32 98, ; 249
	i32 3, ; 250
	i32 42, ; 251
	i32 151, ; 252
	i32 48, ; 253
	i32 11, ; 254
	i32 115, ; 255
	i32 36, ; 256
	i32 62, ; 257
	i32 155, ; 258
	i32 24, ; 259
	i32 23, ; 260
	i32 76, ; 261
	i32 31, ; 262
	i32 126, ; 263
	i32 90, ; 264
	i32 28, ; 265
	i32 95, ; 266
	i32 41, ; 267
	i32 60, ; 268
	i32 155, ; 269
	i32 80, ; 270
	i32 33, ; 271
	i32 94, ; 272
	i32 128, ; 273
	i32 73, ; 274
	i32 82, ; 275
	i32 111, ; 276
	i32 39, ; 277
	i32 48, ; 278
	i32 112, ; 279
	i32 37, ; 280
	i32 143, ; 281
	i32 43, ; 282
	i32 130, ; 283
	i32 137, ; 284
	i32 32, ; 285
	i32 109, ; 286
	i32 84, ; 287
	i32 149, ; 288
	i32 97, ; 289
	i32 78, ; 290
	i32 27, ; 291
	i32 9, ; 292
	i32 118, ; 293
	i32 39, ; 294
	i32 74, ; 295
	i32 127, ; 296
	i32 50, ; 297
	i32 57, ; 298
	i32 147, ; 299
	i32 52, ; 300
	i32 121, ; 301
	i32 135, ; 302
	i32 22, ; 303
	i32 17, ; 304
	i32 42, ; 305
	i32 29, ; 306
	i32 92, ; 307
	i32 73, ; 308
	i32 49, ; 309
	i32 139, ; 310
	i32 92 ; 311
], align 4

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 4

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 4

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 4

; Functions

; Function attributes: "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" uwtable willreturn
define void @xamarin_app_init(ptr nocapture noundef readnone %env, ptr noundef %fn) local_unnamed_addr #0
{
	%fnIsNull = icmp eq ptr %fn, null
	br i1 %fnIsNull, label %1, label %2

1: ; preds = %0
	%putsResult = call noundef i32 @puts(ptr @.str.0)
	call void @abort()
	unreachable 

2: ; preds = %1, %0
	store ptr %fn, ptr @get_function_pointer, align 4, !tbaa !3
	ret void
}

; Strings
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 1

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-thumb-mode,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-thumb-mode,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }

; Metadata
!llvm.module.flags = !{!0, !1, !7}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!"Xamarin.Android remotes/origin/release/8.0.4xx @ df9aaf29a52042a4fbf800daf2f3a38964b9e958"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"min_enum_size", i32 4}
