; ModuleID = 'marshal_methods.x86_64.ll'
source_filename = "marshal_methods.x86_64.ll"
target datalayout = "e-m:e-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [156 x ptr] zeroinitializer, align 16

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [312 x i64] [
	i64 98382396393917666, ; 0: Microsoft.Extensions.Primitives.dll => 0x15d8644ad360ce2 => 47
	i64 120698629574877762, ; 1: Mono.Android => 0x1accec39cafe242 => 155
	i64 131669012237370309, ; 2: Microsoft.Maui.Essentials.dll => 0x1d3c844de55c3c5 => 52
	i64 196720943101637631, ; 3: System.Linq.Expressions.dll => 0x2bae4a7cd73f3ff => 125
	i64 210515253464952879, ; 4: Xamarin.AndroidX.Collection.dll => 0x2ebe681f694702f => 82
	i64 232391251801502327, ; 5: Xamarin.AndroidX.SavedState.dll => 0x3399e9cbc897277 => 99
	i64 308826992458506653, ; 6: SkiaSharp.Extended.dll => 0x4492c836e8aa19d => 55
	i64 435118502366263740, ; 7: Xamarin.AndroidX.Security.SecurityCrypto.dll => 0x609d9f8f8bdb9bc => 100
	i64 545109961164950392, ; 8: fi/Microsoft.Maui.Controls.resources.dll => 0x7909e9f1ec38b78 => 7
	i64 750875890346172408, ; 9: System.Threading.Thread => 0xa6ba5a4da7d1ff8 => 146
	i64 799765834175365804, ; 10: System.ComponentModel.dll => 0xb1956c9f18442ac => 116
	i64 849051935479314978, ; 11: hi/Microsoft.Maui.Controls.resources.dll => 0xbc8703ca21a3a22 => 10
	i64 872800313462103108, ; 12: Xamarin.AndroidX.DrawerLayout => 0xc1ccf42c3c21c44 => 87
	i64 1120440138749646132, ; 13: Xamarin.Google.Android.Material.dll => 0xf8c9a5eae431534 => 104
	i64 1121665720830085036, ; 14: nb/Microsoft.Maui.Controls.resources.dll => 0xf90f507becf47ac => 18
	i64 1168642086743967698, ; 15: Syncfusion.Maui.Buttons.dll => 0x1037d9c941f207d2 => 65
	i64 1268860745194512059, ; 16: System.Drawing.dll => 0x119be62002c19ebb => 121
	i64 1369545283391376210, ; 17: Xamarin.AndroidX.Navigation.Fragment.dll => 0x13019a2dd85acb52 => 95
	i64 1433520707554318520, ; 18: SkiaSharp.Extended.UI.dll => 0x13e4e37d07f118b8 => 56
	i64 1476839205573959279, ; 19: System.Net.Primitives.dll => 0x147ec96ece9b1e6f => 131
	i64 1486715745332614827, ; 20: Microsoft.Maui.Controls.dll => 0x14a1e017ea87d6ab => 49
	i64 1513467482682125403, ; 21: Mono.Android.Runtime => 0x1500eaa8245f6c5b => 154
	i64 1537168428375924959, ; 22: System.Threading.Thread.dll => 0x15551e8a954ae0df => 146
	i64 1556147632182429976, ; 23: ko/Microsoft.Maui.Controls.resources.dll => 0x15988c06d24c8918 => 16
	i64 1559087064654078745, ; 24: BCrypt.Net-Next.dll => 0x15a2fd6cc69ce319 => 36
	i64 1624659445732251991, ; 25: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0x168bf32877da9957 => 79
	i64 1628611045998245443, ; 26: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0x1699fd1e1a00b643 => 92
	i64 1735388228521408345, ; 27: System.Net.Mail.dll => 0x181556663c69b759 => 130
	i64 1743969030606105336, ; 28: System.Memory.dll => 0x1833d297e88f2af8 => 127
	i64 1767386781656293639, ; 29: System.Private.Uri.dll => 0x188704e9f5582107 => 135
	i64 1795316252682057001, ; 30: Xamarin.AndroidX.AppCompat.dll => 0x18ea3e9eac997529 => 78
	i64 1835311033149317475, ; 31: es\Microsoft.Maui.Controls.resources => 0x197855a927386163 => 6
	i64 1836611346387731153, ; 32: Xamarin.AndroidX.SavedState => 0x197cf449ebe482d1 => 99
	i64 1875417405349196092, ; 33: System.Drawing.Primitives => 0x1a06d2319b6c713c => 120
	i64 1881198190668717030, ; 34: tr\Microsoft.Maui.Controls.resources => 0x1a1b5bc992ea9be6 => 28
	i64 1897575647115118287, ; 35: Xamarin.AndroidX.Security.SecurityCrypto => 0x1a558aff4cba86cf => 100
	i64 1920760634179481754, ; 36: Microsoft.Maui.Controls.Xaml => 0x1aa7e99ec2d2709a => 50
	i64 1959996714666907089, ; 37: tr/Microsoft.Maui.Controls.resources.dll => 0x1b334ea0a2a755d1 => 28
	i64 1972385128188460614, ; 38: System.Security.Cryptography.Algorithms => 0x1b5f51d2edefbe46 => 141
	i64 1981742497975770890, ; 39: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x1b80904d5c241f0a => 91
	i64 1983698669889758782, ; 40: cs/Microsoft.Maui.Controls.resources.dll => 0x1b87836e2031a63e => 2
	i64 2019660174692588140, ; 41: pl/Microsoft.Maui.Controls.resources.dll => 0x1c07463a6f8e1a6c => 20
	i64 2102659300918482391, ; 42: System.Drawing.Primitives.dll => 0x1d2e257e6aead5d7 => 120
	i64 2165725771938924357, ; 43: Xamarin.AndroidX.Browser => 0x1e0e341d75540745 => 80
	i64 2188974421706709258, ; 44: SkiaSharp.HarfBuzz.dll => 0x1e60cca38c3e990a => 57
	i64 2262844636196693701, ; 45: Xamarin.AndroidX.DrawerLayout.dll => 0x1f673d352266e6c5 => 87
	i64 2287834202362508563, ; 46: System.Collections.Concurrent => 0x1fc00515e8ce7513 => 109
	i64 2302323944321350744, ; 47: ru/Microsoft.Maui.Controls.resources.dll => 0x1ff37f6ddb267c58 => 24
	i64 2329709569556905518, ; 48: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x2054ca829b447e2e => 90
	i64 2335503487726329082, ; 49: System.Text.Encodings.Web => 0x2069600c4d9d1cfa => 75
	i64 2364395629313652312, ; 50: Syncfusion.Maui.Expander.dll => 0x20d0054c39b9f658 => 68
	i64 2470498323731680442, ; 51: Xamarin.AndroidX.CoordinatorLayout => 0x2248f922dc398cba => 83
	i64 2497223385847772520, ; 52: System.Runtime => 0x22a7eb7046413568 => 140
	i64 2547086958574651984, ; 53: Xamarin.AndroidX.Activity.dll => 0x2359121801df4a50 => 77
	i64 2567318800469729806, ; 54: Syncfusion.Maui.PullToRefresh => 0x23a0f2d8c72e220e => 71
	i64 2602673633151553063, ; 55: th\Microsoft.Maui.Controls.resources => 0x241e8de13a460e27 => 27
	i64 2656907746661064104, ; 56: Microsoft.Extensions.DependencyInjection => 0x24df3b84c8b75da8 => 42
	i64 2662981627730767622, ; 57: cs\Microsoft.Maui.Controls.resources => 0x24f4cfae6c48af06 => 2
	i64 2895129759130297543, ; 58: fi\Microsoft.Maui.Controls.resources => 0x282d912d479fa4c7 => 7
	i64 3017704767998173186, ; 59: Xamarin.Google.Android.Material => 0x29e10a7f7d88a002 => 104
	i64 3289520064315143713, ; 60: Xamarin.AndroidX.Lifecycle.Common => 0x2da6b911e3063621 => 89
	i64 3311221304742556517, ; 61: System.Numerics.Vectors.dll => 0x2df3d23ba9e2b365 => 133
	i64 3325875462027654285, ; 62: System.Runtime.Numerics => 0x2e27e21c8958b48d => 139
	i64 3344514922410554693, ; 63: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x2e6a1a9a18463545 => 107
	i64 3414639567687375782, ; 64: SkiaSharp.Views.Maui.Controls => 0x2f633c9863ffdba6 => 62
	i64 3429672777697402584, ; 65: Microsoft.Maui.Essentials => 0x2f98a5385a7b1ed8 => 52
	i64 3461602852075779363, ; 66: SkiaSharp.HarfBuzz => 0x300a15741f74b523 => 57
	i64 3494946837667399002, ; 67: Microsoft.Extensions.Configuration => 0x30808ba1c00a455a => 40
	i64 3520784151859913863, ; 68: Syncfusion.Maui.TabView.dll => 0x30dc56883e6f6487 => 72
	i64 3522470458906976663, ; 69: Xamarin.AndroidX.SwipeRefreshLayout => 0x30e2543832f52197 => 101
	i64 3551103847008531295, ; 70: System.Private.CoreLib.dll => 0x31480e226177735f => 152
	i64 3567343442040498961, ; 71: pt\Microsoft.Maui.Controls.resources => 0x3181bff5bea4ab11 => 22
	i64 3571415421602489686, ; 72: System.Runtime.dll => 0x319037675df7e556 => 140
	i64 3638003163729360188, ; 73: Microsoft.Extensions.Configuration.Abstractions => 0x327cc89a39d5f53c => 41
	i64 3647754201059316852, ; 74: System.Xml.ReaderWriter => 0x329f6d1e86145474 => 149
	i64 3655542548057982301, ; 75: Microsoft.Extensions.Configuration.dll => 0x32bb18945e52855d => 40
	i64 3716579019761409177, ; 76: netstandard.dll => 0x3393f0ed5c8c5c99 => 151
	i64 3727469159507183293, ; 77: Xamarin.AndroidX.RecyclerView => 0x33baa1739ba646bd => 98
	i64 3869221888984012293, ; 78: Microsoft.Extensions.Logging.dll => 0x35b23cceda0ed605 => 44
	i64 3890352374528606784, ; 79: Microsoft.Maui.Controls.Xaml.dll => 0x35fd4edf66e00240 => 50
	i64 3933965368022646939, ; 80: System.Net.Requests => 0x369840a8bfadc09b => 132
	i64 3966267475168208030, ; 81: System.Memory => 0x370b03412596249e => 127
	i64 4050760258208440355, ; 82: en-US\Syncfusion.Maui.Buttons.resources => 0x383730fe34c8a023 => 34
	i64 4073500526318903918, ; 83: System.Private.Xml.dll => 0x3887fb25779ae26e => 136
	i64 4073631083018132676, ; 84: Microsoft.Maui.Controls.Compatibility.dll => 0x388871e311491cc4 => 48
	i64 4120493066591692148, ; 85: zh-Hant\Microsoft.Maui.Controls.resources => 0x392eee9cdda86574 => 33
	i64 4135615024468428857, ; 86: Syncfusion.Maui.Popup => 0x3964a7f40d358839 => 69
	i64 4154383907710350974, ; 87: System.ComponentModel => 0x39a7562737acb67e => 116
	i64 4187479170553454871, ; 88: System.Linq.Expressions => 0x3a1cea1e912fa117 => 125
	i64 4205801962323029395, ; 89: System.ComponentModel.TypeConverter => 0x3a5e0299f7e7ad93 => 115
	i64 4306612231831054753, ; 90: SkiaSharp.SceneGraph.dll => 0x3bc42901e7a469a1 => 59
	i64 4356591372459378815, ; 91: vi/Microsoft.Maui.Controls.resources.dll => 0x3c75b8c562f9087f => 30
	i64 4477672992252076438, ; 92: System.Web.HttpUtility.dll => 0x3e23e3dcdb8ba196 => 148
	i64 4679594760078841447, ; 93: ar/Microsoft.Maui.Controls.resources.dll => 0x40f142a407475667 => 0
	i64 4794310189461587505, ; 94: Xamarin.AndroidX.Activity => 0x4288cfb749e4c631 => 77
	i64 4795410492532947900, ; 95: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0x428cb86f8f9b7bbc => 101
	i64 4809057822547766521, ; 96: System.Drawing => 0x42bd349c3145ecf9 => 121
	i64 4853321196694829351, ; 97: System.Runtime.Loader.dll => 0x435a75ea15de7927 => 138
	i64 5095776370830346062, ; 98: Syncfusion.Maui.Expander => 0x46b7d59c562c074e => 68
	i64 5103417709280584325, ; 99: System.Collections.Specialized => 0x46d2fb5e161b6285 => 112
	i64 5152918724099874074, ; 100: Syncfusion.Maui.ProgressBar => 0x4782d8473a6e9d1a => 70
	i64 5182934613077526976, ; 101: System.Collections.Specialized.dll => 0x47ed7b91fa9009c0 => 112
	i64 5290786973231294105, ; 102: System.Runtime.Loader => 0x496ca6b869b72699 => 138
	i64 5332349484191854038, ; 103: Syncfusion.Maui.Core.dll => 0x4a004f9a977e2dd6 => 67
	i64 5471532531798518949, ; 104: sv\Microsoft.Maui.Controls.resources => 0x4beec9d926d82ca5 => 26
	i64 5522859530602327440, ; 105: uk\Microsoft.Maui.Controls.resources => 0x4ca5237b51eead90 => 29
	i64 5570799893513421663, ; 106: System.IO.Compression.Brotli => 0x4d4f74fcdfa6c35f => 123
	i64 5573260873512690141, ; 107: System.Security.Cryptography.dll => 0x4d58333c6e4ea1dd => 143
	i64 5650097808083101034, ; 108: System.Security.Cryptography.Algorithms.dll => 0x4e692e055d01a56a => 141
	i64 5692067934154308417, ; 109: Xamarin.AndroidX.ViewPager2.dll => 0x4efe49a0d4a8bb41 => 103
	i64 5979151488806146654, ; 110: System.Formats.Asn1 => 0x52fa3699a489d25e => 122
	i64 5984759512290286505, ; 111: System.Security.Cryptography.Primitives => 0x530e23115c33dba9 => 142
	i64 6068057819846744445, ; 112: ro/Microsoft.Maui.Controls.resources.dll => 0x5436126fec7f197d => 23
	i64 6200764641006662125, ; 113: ro\Microsoft.Maui.Controls.resources => 0x560d8a96830131ed => 23
	i64 6222399776351216807, ; 114: System.Text.Json.dll => 0x565a67a0ffe264a7 => 76
	i64 6268464631992009879, ; 115: SkiaSharp.Skottie => 0x56fe0f5efcfbc497 => 60
	i64 6357457916754632952, ; 116: _Microsoft.Android.Resource.Designer => 0x583a3a4ac2a7a0f8 => 35
	i64 6401687960814735282, ; 117: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0x58d75d486341cfb2 => 90
	i64 6478287442656530074, ; 118: hr\Microsoft.Maui.Controls.resources => 0x59e7801b0c6a8e9a => 11
	i64 6504860066809920875, ; 119: Xamarin.AndroidX.Browser.dll => 0x5a45e7c43bd43d6b => 80
	i64 6548213210057960872, ; 120: Xamarin.AndroidX.CustomView.dll => 0x5adfed387b066da8 => 86
	i64 6560151584539558821, ; 121: Microsoft.Extensions.Options => 0x5b0a571be53243a5 => 46
	i64 6671798237668743565, ; 122: SkiaSharp => 0x5c96fd260152998d => 54
	i64 6743165466166707109, ; 123: nl\Microsoft.Maui.Controls.resources => 0x5d948943c08c43a5 => 19
	i64 6777482997383978746, ; 124: pt/Microsoft.Maui.Controls.resources.dll => 0x5e0e74e0a2525efa => 22
	i64 6784420147581266553, ; 125: en-US/Syncfusion.Maui.Buttons.resources.dll => 0x5e271a2dc795aa79 => 34
	i64 6786606130239981554, ; 126: System.Diagnostics.TraceSource => 0x5e2ede51877147f2 => 119
	i64 6894844156784520562, ; 127: System.Numerics.Vectors => 0x5faf683aead1ad72 => 133
	i64 7220009545223068405, ; 128: sv/Microsoft.Maui.Controls.resources.dll => 0x6432a06d99f35af5 => 26
	i64 7270811800166795866, ; 129: System.Linq => 0x64e71ccf51a90a5a => 126
	i64 7314237870106916923, ; 130: SkiaSharp.Views.Maui.Core.dll => 0x65816497226eb83b => 63
	i64 7377312882064240630, ; 131: System.ComponentModel.TypeConverter.dll => 0x66617afac45a2ff6 => 115
	i64 7489048572193775167, ; 132: System.ObjectModel => 0x67ee71ff6b419e3f => 134
	i64 7592577537120840276, ; 133: System.Diagnostics.Process => 0x695e410af5b2aa54 => 118
	i64 7654504624184590948, ; 134: System.Net.Http => 0x6a3a4366801b8264 => 129
	i64 7694700312542370399, ; 135: System.Net.Mail => 0x6ac9112a7e2cda5f => 130
	i64 7708790323521193081, ; 136: ms/Microsoft.Maui.Controls.resources.dll => 0x6afb1ff4d1730479 => 17
	i64 7714652370974252055, ; 137: System.Private.CoreLib => 0x6b0ff375198b9c17 => 152
	i64 7723873813026311384, ; 138: SkiaSharp.Views.Maui.Controls.dll => 0x6b30b64f63600cd8 => 62
	i64 7735352534559001595, ; 139: Xamarin.Kotlin.StdLib.dll => 0x6b597e2582ce8bfb => 106
	i64 7769135412902976898, ; 140: Syncfusion.Maui.Popup.dll => 0x6bd1837ed1fd5d82 => 69
	i64 7836164640616011524, ; 141: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x6cbfa6390d64d704 => 79
	i64 7927939710195668715, ; 142: SkiaSharp.Views.Android.dll => 0x6e05b32992ed16eb => 61
	i64 8064050204834738623, ; 143: System.Collections.dll => 0x6fe942efa61731bf => 113
	i64 8083354569033831015, ; 144: Xamarin.AndroidX.Lifecycle.Common.dll => 0x702dd82730cad267 => 89
	i64 8085230611270010360, ; 145: System.Net.Http.Json.dll => 0x703482674fdd05f8 => 128
	i64 8087206902342787202, ; 146: System.Diagnostics.DiagnosticSource => 0x703b87d46f3aa082 => 73
	i64 8167236081217502503, ; 147: Java.Interop.dll => 0x7157d9f1a9b8fd27 => 153
	i64 8185542183669246576, ; 148: System.Collections => 0x7198e33f4794aa70 => 113
	i64 8246048515196606205, ; 149: Microsoft.Maui.Graphics.dll => 0x726fd96f64ee56fd => 53
	i64 8264926008854159966, ; 150: System.Diagnostics.Process.dll => 0x72b2ea6a64a3a25e => 118
	i64 8368701292315763008, ; 151: System.Security.Cryptography => 0x7423997c6fd56140 => 143
	i64 8400357532724379117, ; 152: Xamarin.AndroidX.Navigation.UI.dll => 0x749410ab44503ded => 97
	i64 8476828615142258695, ; 153: BCrypt.Net-Next => 0x75a3beb69b6bb807 => 36
	i64 8518412311883997971, ; 154: System.Collections.Immutable => 0x76377add7c28e313 => 110
	i64 8542321783515278838, ; 155: Syncfusion.Maui.TabView => 0x768c6c6727826df6 => 72
	i64 8563666267364444763, ; 156: System.Private.Uri => 0x76d841191140ca5b => 135
	i64 8599632406834268464, ; 157: CommunityToolkit.Maui => 0x7758081c784b4930 => 37
	i64 8614108721271900878, ; 158: pt-BR/Microsoft.Maui.Controls.resources.dll => 0x778b763e14018ace => 21
	i64 8626175481042262068, ; 159: Java.Interop => 0x77b654e585b55834 => 153
	i64 8639588376636138208, ; 160: Xamarin.AndroidX.Navigation.Runtime => 0x77e5fbdaa2fda2e0 => 96
	i64 8677882282824630478, ; 161: pt-BR\Microsoft.Maui.Controls.resources => 0x786e07f5766b00ce => 21
	i64 8725526185868997716, ; 162: System.Diagnostics.DiagnosticSource.dll => 0x79174bd613173454 => 73
	i64 9045785047181495996, ; 163: zh-HK\Microsoft.Maui.Controls.resources => 0x7d891592e3cb0ebc => 31
	i64 9312692141327339315, ; 164: Xamarin.AndroidX.ViewPager2 => 0x813d54296a634f33 => 103
	i64 9324707631942237306, ; 165: Xamarin.AndroidX.AppCompat => 0x8168042fd44a7c7a => 78
	i64 9334813198578103615, ; 166: SkiaSharp.Extended.UI => 0x818beb2569e0353f => 56
	i64 9575902398040817096, ; 167: Xamarin.Google.Crypto.Tink.Android.dll => 0x84e4707ee708bdc8 => 105
	i64 9659729154652888475, ; 168: System.Text.RegularExpressions => 0x860e407c9991dd9b => 145
	i64 9678050649315576968, ; 169: Xamarin.AndroidX.CoordinatorLayout.dll => 0x864f57c9feb18c88 => 83
	i64 9702891218465930390, ; 170: System.Collections.NonGeneric.dll => 0x86a79827b2eb3c96 => 111
	i64 9807524880124547146, ; 171: SnackUpClient => 0x881b53ea7da1104a => 108
	i64 9808709177481450983, ; 172: Mono.Android.dll => 0x881f890734e555e7 => 155
	i64 9956195530459977388, ; 173: Microsoft.Maui => 0x8a2b8315b36616ac => 51
	i64 9991543690424095600, ; 174: es/Microsoft.Maui.Controls.resources.dll => 0x8aa9180c89861370 => 6
	i64 10038780035334861115, ; 175: System.Net.Http.dll => 0x8b50e941206af13b => 129
	i64 10051358222726253779, ; 176: System.Private.Xml => 0x8b7d990c97ccccd3 => 136
	i64 10065192478696882816, ; 177: Syncfusion.Maui.PullToRefresh.dll => 0x8baebf3b50a94280 => 71
	i64 10092835686693276772, ; 178: Microsoft.Maui.Controls => 0x8c10f49539bd0c64 => 49
	i64 10143853363526200146, ; 179: da\Microsoft.Maui.Controls.resources => 0x8cc634e3c2a16b52 => 3
	i64 10229024438826829339, ; 180: Xamarin.AndroidX.CustomView => 0x8df4cb880b10061b => 86
	i64 10331071620080664917, ; 181: SkiaSharp.Resources => 0x8f5f56e6f0070155 => 58
	i64 10406448008575299332, ; 182: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x906b2153fcb3af04 => 107
	i64 10430153318873392755, ; 183: Xamarin.AndroidX.Core => 0x90bf592ea44f6673 => 84
	i64 10506226065143327199, ; 184: ca\Microsoft.Maui.Controls.resources => 0x91cd9cf11ed169df => 1
	i64 10618541785153769341, ; 185: Syncfusion.Maui.Calendar => 0x935ca37e80bb8f7d => 66
	i64 10642885958238400069, ; 186: Syncfusion.Maui.Calendar.dll => 0x93b32063fdcaee45 => 66
	i64 10785150219063592792, ; 187: System.Net.Primitives => 0x95ac8cfb68830758 => 131
	i64 10880838204485145808, ; 188: CommunityToolkit.Maui.dll => 0x970080b2a4d614d0 => 37
	i64 11002576679268595294, ; 189: Microsoft.Extensions.Logging.Abstractions => 0x98b1013215cd365e => 45
	i64 11009005086950030778, ; 190: Microsoft.Maui.dll => 0x98c7d7cc621ffdba => 51
	i64 11103970607964515343, ; 191: hu\Microsoft.Maui.Controls.resources => 0x9a193a6fc41a6c0f => 12
	i64 11162124722117608902, ; 192: Xamarin.AndroidX.ViewPager => 0x9ae7d54b986d05c6 => 102
	i64 11220793807500858938, ; 193: ja\Microsoft.Maui.Controls.resources => 0x9bb8448481fdd63a => 15
	i64 11226290749488709958, ; 194: Microsoft.Extensions.Options.dll => 0x9bcbcbf50c874146 => 46
	i64 11269959613938147231, ; 195: SnackUpClient.dll => 0x9c66f08eddc81f9f => 108
	i64 11340910727871153756, ; 196: Xamarin.AndroidX.CursorAdapter => 0x9d630238642d465c => 85
	i64 11485890710487134646, ; 197: System.Runtime.InteropServices => 0x9f6614bf0f8b71b6 => 137
	i64 11513602507638267977, ; 198: System.IO.Pipelines.dll => 0x9fc8887aa0d36049 => 74
	i64 11518296021396496455, ; 199: id\Microsoft.Maui.Controls.resources => 0x9fd9353475222047 => 13
	i64 11529969570048099689, ; 200: Xamarin.AndroidX.ViewPager.dll => 0xa002ae3c4dc7c569 => 102
	i64 11530571088791430846, ; 201: Microsoft.Extensions.Logging => 0xa004d1504ccd66be => 44
	i64 11562066371261255856, ; 202: SkiaSharp.Resources.dll => 0xa074b61b308450b0 => 58
	i64 11597940890313164233, ; 203: netstandard => 0xa0f429ca8d1805c9 => 151
	i64 11705530742807338875, ; 204: he/Microsoft.Maui.Controls.resources.dll => 0xa272663128721f7b => 9
	i64 12145679461940342714, ; 205: System.Text.Json => 0xa88e1f1ebcb62fba => 76
	i64 12269460666702402136, ; 206: System.Collections.Immutable.dll => 0xaa45e178506c9258 => 110
	i64 12313367145828839434, ; 207: System.IO.Pipelines => 0xaae1de2e1c17f00a => 74
	i64 12341818387765915815, ; 208: CommunityToolkit.Maui.Core.dll => 0xab46f26f152bf0a7 => 38
	i64 12451044538927396471, ; 209: Xamarin.AndroidX.Fragment.dll => 0xaccaff0a2955b677 => 88
	i64 12466513435562512481, ; 210: Xamarin.AndroidX.Loader.dll => 0xad01f3eb52569061 => 93
	i64 12475113361194491050, ; 211: _Microsoft.Android.Resource.Designer.dll => 0xad2081818aba1caa => 35
	i64 12517810545449516888, ; 212: System.Diagnostics.TraceSource.dll => 0xadb8325e6f283f58 => 119
	i64 12538491095302438457, ; 213: Xamarin.AndroidX.CardView.dll => 0xae01ab382ae67e39 => 81
	i64 12550732019250633519, ; 214: System.IO.Compression => 0xae2d28465e8e1b2f => 124
	i64 12681088699309157496, ; 215: it/Microsoft.Maui.Controls.resources.dll => 0xaffc46fc178aec78 => 14
	i64 12700543734426720211, ; 216: Xamarin.AndroidX.Collection => 0xb041653c70d157d3 => 82
	i64 12708922737231849740, ; 217: System.Text.Encoding.Extensions => 0xb05f29e50e96e90c => 144
	i64 12823819093633476069, ; 218: th/Microsoft.Maui.Controls.resources.dll => 0xb1f75b85abe525e5 => 27
	i64 12843321153144804894, ; 219: Microsoft.Extensions.Primitives => 0xb23ca48abd74d61e => 47
	i64 13106026140046202731, ; 220: HarfBuzzSharp.dll => 0xb5e1f555ee70176b => 39
	i64 13221551921002590604, ; 221: ca/Microsoft.Maui.Controls.resources.dll => 0xb77c636bdebe318c => 1
	i64 13222659110913276082, ; 222: ja/Microsoft.Maui.Controls.resources.dll => 0xb78052679c1178b2 => 15
	i64 13343850469010654401, ; 223: Mono.Android.Runtime.dll => 0xb92ee14d854f44c1 => 154
	i64 13381594904270902445, ; 224: he\Microsoft.Maui.Controls.resources => 0xb9b4f9aaad3e94ad => 9
	i64 13465488254036897740, ; 225: Xamarin.Kotlin.StdLib => 0xbadf06394d106fcc => 106
	i64 13467053111158216594, ; 226: uk/Microsoft.Maui.Controls.resources.dll => 0xbae49573fde79792 => 29
	i64 13540124433173649601, ; 227: vi\Microsoft.Maui.Controls.resources => 0xbbe82f6eede718c1 => 30
	i64 13545416393490209236, ; 228: id/Microsoft.Maui.Controls.resources.dll => 0xbbfafc7174bc99d4 => 13
	i64 13572454107664307259, ; 229: Xamarin.AndroidX.RecyclerView.dll => 0xbc5b0b19d99f543b => 98
	i64 13717397318615465333, ; 230: System.ComponentModel.Primitives.dll => 0xbe5dfc2ef2f87d75 => 114
	i64 13755568601956062840, ; 231: fr/Microsoft.Maui.Controls.resources.dll => 0xbee598c36b1b9678 => 8
	i64 13814445057219246765, ; 232: hr/Microsoft.Maui.Controls.resources.dll => 0xbfb6c49664b43aad => 11
	i64 13881769479078963060, ; 233: System.Console.dll => 0xc0a5f3cade5c6774 => 117
	i64 13959074834287824816, ; 234: Xamarin.AndroidX.Fragment => 0xc1b8989a7ad20fb0 => 88
	i64 13970307180132182141, ; 235: Syncfusion.Licensing => 0xc1e0805ccade287d => 64
	i64 14100563506285742564, ; 236: da/Microsoft.Maui.Controls.resources.dll => 0xc3af43cd0cff89e4 => 3
	i64 14124974489674258913, ; 237: Xamarin.AndroidX.CardView => 0xc405fd76067d19e1 => 81
	i64 14125464355221830302, ; 238: System.Threading.dll => 0xc407bafdbc707a9e => 147
	i64 14254574811015963973, ; 239: System.Text.Encoding.Extensions.dll => 0xc5d26c4442d66545 => 144
	i64 14461014870687870182, ; 240: System.Net.Requests.dll => 0xc8afd8683afdece6 => 132
	i64 14464374589798375073, ; 241: ru\Microsoft.Maui.Controls.resources => 0xc8bbc80dcb1e5ea1 => 24
	i64 14522721392235705434, ; 242: el/Microsoft.Maui.Controls.resources.dll => 0xc98b12295c2cf45a => 5
	i64 14538127318538747197, ; 243: Syncfusion.Licensing.dll => 0xc9c1cdc518e77d3d => 64
	i64 14551742072151931844, ; 244: System.Text.Encodings.Web.dll => 0xc9f22c50f1b8fbc4 => 75
	i64 14552901170081803662, ; 245: SkiaSharp.Views.Maui.Core => 0xc9f64a827617ad8e => 63
	i64 14556034074661724008, ; 246: CommunityToolkit.Maui.Core => 0xca016bdea6b19f68 => 38
	i64 14561513370130550166, ; 247: System.Security.Cryptography.Primitives.dll => 0xca14e3428abb8d96 => 142
	i64 14669215534098758659, ; 248: Microsoft.Extensions.DependencyInjection.dll => 0xcb9385ceb3993c03 => 42
	i64 14690985099581930927, ; 249: System.Web.HttpUtility => 0xcbe0dd1ca5233daf => 148
	i64 14705122255218365489, ; 250: ko\Microsoft.Maui.Controls.resources => 0xcc1316c7b0fb5431 => 16
	i64 14744092281598614090, ; 251: zh-Hans\Microsoft.Maui.Controls.resources => 0xcc9d89d004439a4a => 32
	i64 14852515768018889994, ; 252: Xamarin.AndroidX.CursorAdapter.dll => 0xce1ebc6625a76d0a => 85
	i64 14892012299694389861, ; 253: zh-Hant/Microsoft.Maui.Controls.resources.dll => 0xceab0e490a083a65 => 33
	i64 14904040806490515477, ; 254: ar\Microsoft.Maui.Controls.resources => 0xced5ca2604cb2815 => 0
	i64 14931407803744742450, ; 255: HarfBuzzSharp => 0xcf3704499ab36c32 => 39
	i64 14954917835170835695, ; 256: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xcf8a8a895a82ecef => 43
	i64 14987728460634540364, ; 257: System.IO.Compression.dll => 0xcfff1ba06622494c => 124
	i64 15024878362326791334, ; 258: System.Net.Http.Json => 0xd0831743ebf0f4a6 => 128
	i64 15076659072870671916, ; 259: System.ObjectModel.dll => 0xd13b0d8c1620662c => 134
	i64 15101927338945785474, ; 260: SkiaSharp.SceneGraph => 0xd194d2e6bd9fae82 => 59
	i64 15111608613780139878, ; 261: ms\Microsoft.Maui.Controls.resources => 0xd1b737f831192f66 => 17
	i64 15115185479366240210, ; 262: System.IO.Compression.Brotli.dll => 0xd1c3ed1c1bc467d2 => 123
	i64 15133485256822086103, ; 263: System.Linq.dll => 0xd204f0a9127dd9d7 => 126
	i64 15227001540531775957, ; 264: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd3512d3999b8e9d5 => 41
	i64 15370334346939861994, ; 265: Xamarin.AndroidX.Core.dll => 0xd54e65a72c560bea => 84
	i64 15391712275433856905, ; 266: Microsoft.Extensions.DependencyInjection.Abstractions => 0xd59a58c406411f89 => 43
	i64 15527772828719725935, ; 267: System.Console => 0xd77dbb1e38cd3d6f => 117
	i64 15536481058354060254, ; 268: de\Microsoft.Maui.Controls.resources => 0xd79cab34eec75bde => 4
	i64 15582737692548360875, ; 269: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xd841015ed86f6aab => 92
	i64 15609085926864131306, ; 270: System.dll => 0xd89e9cf3334914ea => 150
	i64 15661133872274321916, ; 271: System.Xml.ReaderWriter.dll => 0xd9578647d4bfb1fc => 149
	i64 15664356999916475676, ; 272: de/Microsoft.Maui.Controls.resources.dll => 0xd962f9b2b6ecd51c => 4
	i64 15743187114543869802, ; 273: hu/Microsoft.Maui.Controls.resources.dll => 0xda7b09450ae4ef6a => 12
	i64 15745825835632158716, ; 274: Syncfusion.Maui.Core => 0xda84692c2c05e7fc => 67
	i64 15783653065526199428, ; 275: el\Microsoft.Maui.Controls.resources => 0xdb0accd674b1c484 => 5
	i64 15928521404965645318, ; 276: Microsoft.Maui.Controls.Compatibility => 0xdd0d79d32c2eec06 => 48
	i64 16154507427712707110, ; 277: System => 0xe03056ea4e39aa26 => 150
	i64 16288847719894691167, ; 278: nb\Microsoft.Maui.Controls.resources => 0xe20d9cb300c12d5f => 18
	i64 16321164108206115771, ; 279: Microsoft.Extensions.Logging.Abstractions.dll => 0xe2806c487e7b0bbb => 45
	i64 16324796876805858114, ; 280: SkiaSharp.dll => 0xe28d5444586b6342 => 54
	i64 16649148416072044166, ; 281: Microsoft.Maui.Graphics => 0xe70da84600bb4e86 => 53
	i64 16677317093839702854, ; 282: Xamarin.AndroidX.Navigation.UI => 0xe771bb8960dd8b46 => 97
	i64 16890310621557459193, ; 283: System.Text.RegularExpressions.dll => 0xea66700587f088f9 => 145
	i64 16942731696432749159, ; 284: sk\Microsoft.Maui.Controls.resources => 0xeb20acb622a01a67 => 25
	i64 16961387572093531548, ; 285: SkiaSharp.Extended => 0xeb62f421ac5c359c => 55
	i64 16998075588627545693, ; 286: Xamarin.AndroidX.Navigation.Fragment => 0xebe54bb02d623e5d => 95
	i64 17008137082415910100, ; 287: System.Collections.NonGeneric => 0xec090a90408c8cd4 => 111
	i64 17031351772568316411, ; 288: Xamarin.AndroidX.Navigation.Common.dll => 0xec5b843380a769fb => 94
	i64 17062143951396181894, ; 289: System.ComponentModel.Primitives => 0xecc8e986518c9786 => 114
	i64 17089008752050867324, ; 290: zh-Hans/Microsoft.Maui.Controls.resources.dll => 0xed285aeb25888c7c => 32
	i64 17102494345910816168, ; 291: Syncfusion.Maui.Buttons => 0xed5843fea5240da8 => 65
	i64 17342750010158924305, ; 292: hi\Microsoft.Maui.Controls.resources => 0xf0add33f97ecc211 => 10
	i64 17360349973592121190, ; 293: Xamarin.Google.Crypto.Tink.Android => 0xf0ec5a52686b9f66 => 105
	i64 17438153253682247751, ; 294: sk/Microsoft.Maui.Controls.resources.dll => 0xf200c3fe308d7847 => 25
	i64 17514990004910432069, ; 295: fr\Microsoft.Maui.Controls.resources => 0xf311be9c6f341f45 => 8
	i64 17623389608345532001, ; 296: pl\Microsoft.Maui.Controls.resources => 0xf492db79dfbef661 => 20
	i64 17671501775136238532, ; 297: Syncfusion.Maui.ProgressBar.dll => 0xf53dc93ca2ae2bc4 => 70
	i64 17671790519499593115, ; 298: SkiaSharp.Views.Android => 0xf53ecfd92be3959b => 61
	i64 17702523067201099846, ; 299: zh-HK/Microsoft.Maui.Controls.resources.dll => 0xf5abfef008ae1846 => 31
	i64 17704177640604968747, ; 300: Xamarin.AndroidX.Loader => 0xf5b1dfc36cac272b => 93
	i64 17710060891934109755, ; 301: Xamarin.AndroidX.Lifecycle.ViewModel => 0xf5c6c68c9e45303b => 91
	i64 17712670374920797664, ; 302: System.Runtime.InteropServices.dll => 0xf5d00bdc38bd3de0 => 137
	i64 17777860260071588075, ; 303: System.Runtime.Numerics.dll => 0xf6b7a5b72419c0eb => 139
	i64 17808848867378959707, ; 304: SkiaSharp.Skottie.dll => 0xf725bdb086bd955b => 60
	i64 18025913125965088385, ; 305: System.Threading => 0xfa28e87b91334681 => 147
	i64 18099568558057551825, ; 306: nl/Microsoft.Maui.Controls.resources.dll => 0xfb2e95b53ad977d1 => 19
	i64 18121036031235206392, ; 307: Xamarin.AndroidX.Navigation.Common => 0xfb7ada42d3d42cf8 => 94
	i64 18146411883821974900, ; 308: System.Formats.Asn1.dll => 0xfbd50176eb22c574 => 122
	i64 18245806341561545090, ; 309: System.Collections.Concurrent.dll => 0xfd3620327d587182 => 109
	i64 18305135509493619199, ; 310: Xamarin.AndroidX.Navigation.Runtime.dll => 0xfe08e7c2d8c199ff => 96
	i64 18324163916253801303 ; 311: it\Microsoft.Maui.Controls.resources => 0xfe4c81ff0a56ab57 => 14
], align 16

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [312 x i32] [
	i32 47, ; 0
	i32 155, ; 1
	i32 52, ; 2
	i32 125, ; 3
	i32 82, ; 4
	i32 99, ; 5
	i32 55, ; 6
	i32 100, ; 7
	i32 7, ; 8
	i32 146, ; 9
	i32 116, ; 10
	i32 10, ; 11
	i32 87, ; 12
	i32 104, ; 13
	i32 18, ; 14
	i32 65, ; 15
	i32 121, ; 16
	i32 95, ; 17
	i32 56, ; 18
	i32 131, ; 19
	i32 49, ; 20
	i32 154, ; 21
	i32 146, ; 22
	i32 16, ; 23
	i32 36, ; 24
	i32 79, ; 25
	i32 92, ; 26
	i32 130, ; 27
	i32 127, ; 28
	i32 135, ; 29
	i32 78, ; 30
	i32 6, ; 31
	i32 99, ; 32
	i32 120, ; 33
	i32 28, ; 34
	i32 100, ; 35
	i32 50, ; 36
	i32 28, ; 37
	i32 141, ; 38
	i32 91, ; 39
	i32 2, ; 40
	i32 20, ; 41
	i32 120, ; 42
	i32 80, ; 43
	i32 57, ; 44
	i32 87, ; 45
	i32 109, ; 46
	i32 24, ; 47
	i32 90, ; 48
	i32 75, ; 49
	i32 68, ; 50
	i32 83, ; 51
	i32 140, ; 52
	i32 77, ; 53
	i32 71, ; 54
	i32 27, ; 55
	i32 42, ; 56
	i32 2, ; 57
	i32 7, ; 58
	i32 104, ; 59
	i32 89, ; 60
	i32 133, ; 61
	i32 139, ; 62
	i32 107, ; 63
	i32 62, ; 64
	i32 52, ; 65
	i32 57, ; 66
	i32 40, ; 67
	i32 72, ; 68
	i32 101, ; 69
	i32 152, ; 70
	i32 22, ; 71
	i32 140, ; 72
	i32 41, ; 73
	i32 149, ; 74
	i32 40, ; 75
	i32 151, ; 76
	i32 98, ; 77
	i32 44, ; 78
	i32 50, ; 79
	i32 132, ; 80
	i32 127, ; 81
	i32 34, ; 82
	i32 136, ; 83
	i32 48, ; 84
	i32 33, ; 85
	i32 69, ; 86
	i32 116, ; 87
	i32 125, ; 88
	i32 115, ; 89
	i32 59, ; 90
	i32 30, ; 91
	i32 148, ; 92
	i32 0, ; 93
	i32 77, ; 94
	i32 101, ; 95
	i32 121, ; 96
	i32 138, ; 97
	i32 68, ; 98
	i32 112, ; 99
	i32 70, ; 100
	i32 112, ; 101
	i32 138, ; 102
	i32 67, ; 103
	i32 26, ; 104
	i32 29, ; 105
	i32 123, ; 106
	i32 143, ; 107
	i32 141, ; 108
	i32 103, ; 109
	i32 122, ; 110
	i32 142, ; 111
	i32 23, ; 112
	i32 23, ; 113
	i32 76, ; 114
	i32 60, ; 115
	i32 35, ; 116
	i32 90, ; 117
	i32 11, ; 118
	i32 80, ; 119
	i32 86, ; 120
	i32 46, ; 121
	i32 54, ; 122
	i32 19, ; 123
	i32 22, ; 124
	i32 34, ; 125
	i32 119, ; 126
	i32 133, ; 127
	i32 26, ; 128
	i32 126, ; 129
	i32 63, ; 130
	i32 115, ; 131
	i32 134, ; 132
	i32 118, ; 133
	i32 129, ; 134
	i32 130, ; 135
	i32 17, ; 136
	i32 152, ; 137
	i32 62, ; 138
	i32 106, ; 139
	i32 69, ; 140
	i32 79, ; 141
	i32 61, ; 142
	i32 113, ; 143
	i32 89, ; 144
	i32 128, ; 145
	i32 73, ; 146
	i32 153, ; 147
	i32 113, ; 148
	i32 53, ; 149
	i32 118, ; 150
	i32 143, ; 151
	i32 97, ; 152
	i32 36, ; 153
	i32 110, ; 154
	i32 72, ; 155
	i32 135, ; 156
	i32 37, ; 157
	i32 21, ; 158
	i32 153, ; 159
	i32 96, ; 160
	i32 21, ; 161
	i32 73, ; 162
	i32 31, ; 163
	i32 103, ; 164
	i32 78, ; 165
	i32 56, ; 166
	i32 105, ; 167
	i32 145, ; 168
	i32 83, ; 169
	i32 111, ; 170
	i32 108, ; 171
	i32 155, ; 172
	i32 51, ; 173
	i32 6, ; 174
	i32 129, ; 175
	i32 136, ; 176
	i32 71, ; 177
	i32 49, ; 178
	i32 3, ; 179
	i32 86, ; 180
	i32 58, ; 181
	i32 107, ; 182
	i32 84, ; 183
	i32 1, ; 184
	i32 66, ; 185
	i32 66, ; 186
	i32 131, ; 187
	i32 37, ; 188
	i32 45, ; 189
	i32 51, ; 190
	i32 12, ; 191
	i32 102, ; 192
	i32 15, ; 193
	i32 46, ; 194
	i32 108, ; 195
	i32 85, ; 196
	i32 137, ; 197
	i32 74, ; 198
	i32 13, ; 199
	i32 102, ; 200
	i32 44, ; 201
	i32 58, ; 202
	i32 151, ; 203
	i32 9, ; 204
	i32 76, ; 205
	i32 110, ; 206
	i32 74, ; 207
	i32 38, ; 208
	i32 88, ; 209
	i32 93, ; 210
	i32 35, ; 211
	i32 119, ; 212
	i32 81, ; 213
	i32 124, ; 214
	i32 14, ; 215
	i32 82, ; 216
	i32 144, ; 217
	i32 27, ; 218
	i32 47, ; 219
	i32 39, ; 220
	i32 1, ; 221
	i32 15, ; 222
	i32 154, ; 223
	i32 9, ; 224
	i32 106, ; 225
	i32 29, ; 226
	i32 30, ; 227
	i32 13, ; 228
	i32 98, ; 229
	i32 114, ; 230
	i32 8, ; 231
	i32 11, ; 232
	i32 117, ; 233
	i32 88, ; 234
	i32 64, ; 235
	i32 3, ; 236
	i32 81, ; 237
	i32 147, ; 238
	i32 144, ; 239
	i32 132, ; 240
	i32 24, ; 241
	i32 5, ; 242
	i32 64, ; 243
	i32 75, ; 244
	i32 63, ; 245
	i32 38, ; 246
	i32 142, ; 247
	i32 42, ; 248
	i32 148, ; 249
	i32 16, ; 250
	i32 32, ; 251
	i32 85, ; 252
	i32 33, ; 253
	i32 0, ; 254
	i32 39, ; 255
	i32 43, ; 256
	i32 124, ; 257
	i32 128, ; 258
	i32 134, ; 259
	i32 59, ; 260
	i32 17, ; 261
	i32 123, ; 262
	i32 126, ; 263
	i32 41, ; 264
	i32 84, ; 265
	i32 43, ; 266
	i32 117, ; 267
	i32 4, ; 268
	i32 92, ; 269
	i32 150, ; 270
	i32 149, ; 271
	i32 4, ; 272
	i32 12, ; 273
	i32 67, ; 274
	i32 5, ; 275
	i32 48, ; 276
	i32 150, ; 277
	i32 18, ; 278
	i32 45, ; 279
	i32 54, ; 280
	i32 53, ; 281
	i32 97, ; 282
	i32 145, ; 283
	i32 25, ; 284
	i32 55, ; 285
	i32 95, ; 286
	i32 111, ; 287
	i32 94, ; 288
	i32 114, ; 289
	i32 32, ; 290
	i32 65, ; 291
	i32 10, ; 292
	i32 105, ; 293
	i32 25, ; 294
	i32 8, ; 295
	i32 20, ; 296
	i32 70, ; 297
	i32 61, ; 298
	i32 31, ; 299
	i32 93, ; 300
	i32 91, ; 301
	i32 137, ; 302
	i32 139, ; 303
	i32 60, ; 304
	i32 147, ; 305
	i32 19, ; 306
	i32 94, ; 307
	i32 122, ; 308
	i32 109, ; 309
	i32 96, ; 310
	i32 14 ; 311
], align 16

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 8

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 8

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 8

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
	store ptr %fn, ptr @get_function_pointer, align 8, !tbaa !3
	ret void
}

; Strings
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 16

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+crc32,+cx16,+cx8,+fxsr,+mmx,+popcnt,+sse,+sse2,+sse3,+sse4.1,+sse4.2,+ssse3,+x87" "tune-cpu"="generic" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+crc32,+cx16,+cx8,+fxsr,+mmx,+popcnt,+sse,+sse2,+sse3,+sse4.1,+sse4.2,+ssse3,+x87" "tune-cpu"="generic" }

; Metadata
!llvm.module.flags = !{!0, !1}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!"Xamarin.Android remotes/origin/release/8.0.4xx @ df9aaf29a52042a4fbf800daf2f3a38964b9e958"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
