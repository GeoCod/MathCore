﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Linq;

using static MathCore.SpecialFunctions.Distribution.Student;

namespace MathCore.Tests.SpecialFunctions.Distribution
{
    [TestClass]
    public class StudentTests
    {
        //[TestInitialize]
        //public void Initialize(TestContext Context)
        //{
        //    Context.
        //}

        [TestMethod, TestCategory("Random numbers")]
        public void QuantileHi2Test()
        {
            var P = Enumerable.Range(1, 19).Select(i => i * 5 / 100d).ToArray();
            var K = Enumerable.Range(1, 10).ToArray();

            var hi_with_p_greater_than_05 = QuantileHi2(0.95, 8);
            var hi_with_p_less_than_05 = QuantileHi2(0.05, 8);

            Assert.That.Value(hi_with_p_greater_than_05).IsEqual(15.506278896843497);
            Assert.That.Value(hi_with_p_greater_than_05).IsEqual(15.507313055865437, 1.035e-3);

            Assert.That.Value(hi_with_p_less_than_05).IsEqual(2.7356377218788865);
            Assert.That.Value(hi_with_p_less_than_05).IsEqual(2.732636793499664, 3.01e-3);


            //var M = new double[P.Length, K.Length];
            //for (var p = 0; p < P.Length; p++)
            //    for (var k = 0; k < K.Length; k++)
            //        M[p, k] = QuantileHi2(P[p], K[k]);

            //double[,] M0 =
            //{
            //   { 3.932e-3, 0.103, 0.352, 0.711, 1.145, 1.635,  2.167,  2.733,  3.325,  3.94,   },
            //   { 0.016,    0.211, 0.584, 1.064, 1.61,  2.204,  2.833,  3.49,   4.168,  4.865,  },
            //   { 0.036,    0.325, 0.798, 1.366, 1.994, 2.661,  3.358,  4.078,  4.817,  5.57,   },
            //   { 0.064,    0.446, 1.005, 1.649, 2.343, 3.07,   3.822,  4.594,  5.38,   6.179,  },
            //   { 0.102,    0.575, 1.213, 1.923, 2.675, 3.455,  4.255,  5.071,  5.899,  6.737,  },
            //   { 0.148,    0.713, 1.424, 2.195, 3,     3.828,  4.671,  5.527,  6.393,  7.267,  },
            //   { 0.206,    0.862, 1.642, 2.47,  3.325, 4.197,  5.082,  5.975,  6.876,  7.783,  },
            //   { 0.275,    1.022, 1.869, 2.753, 3.655, 4.57,   5.493,  6.423,  7.357,  8.295,  },
            //   { 0.357,    1.196, 2.109, 3.047, 3.996, 4.952,  5.913,  6.877,  7.843,  8.812,  },
            //   { 0.455,    1.386, 2.366, 3.357, 4.351, 5.348,  6.346,  7.344,  8.343,  9.342,  },
            //   { 0.571,    1.597, 2.643, 3.687, 4.728, 5.765,  6.8,    7.833,  8.863,  9.892,  },
            //   { 0.708,    1.833, 2.946, 4.045, 5.132, 6.211,  7.283,  8.351,  9.414,  10.473, },
            //   { 0.873,    2.1,   3.283, 4.438, 5.573, 6.695,  7.806,  8.909,  10.006, 11.097, },
            //   { 1.074,    2.408, 3.665, 4.878, 6.064, 7.231,  8.383,  9.524,  10.656, 11.781, },
            //   { 1.323,    2.773, 4.108, 5.385, 6.626, 7.841,  9.037,  10.219, 11.389, 12.549, },
            //   { 1.642,    3.219, 4.642, 5.989, 7.289, 8.558,  9.803,  11.03,  12.242, 13.442, },
            //   { 2.072,    3.794, 5.317, 6.745, 8.115, 9.446,  10.748, 12.027, 13.288, 14.534, },
            //   { 2.706,    4.605, 6.251, 7.779, 9.236, 10.645, 12.017, 13.362, 14.684, 15.987, },
            //   { 3.841,    5.991, 7.815, 9.488, 11.07, 12.592, 14.067, 15.507, 16.919, 18.307  },
            //};
        }

        private const double __QuantileHi2ValuesTestAccuracy = 4.7e-3;
        [DataTestMethod]
        [DataRow(0.95, 14, 23.682709800618273, 1.0e-24, DisplayName = "p:0.95, k:14")]
        [DataRow(0.95, 8, 15.507313055865437, 1.035e-3, DisplayName = "p:0.95, k:8 - p > 0.5")]
        [DataRow(0.05, 8, 2.732636793499664, 3.01e-3, DisplayName = "p:0.05, k:8 - p < 0.5")]
        [DataRow(0.01, 10, 2.5582, __QuantileHi2ValuesTestAccuracy, DisplayName = "p:0.01, k:10")]
        [DataRow(0.025, 10, 3.2470, __QuantileHi2ValuesTestAccuracy, DisplayName = "p:0.025, k:10")]
        [DataRow(0.05, 10, 3.9403, __QuantileHi2ValuesTestAccuracy, DisplayName = "p:0.05, k:10")]
        [DataRow(0.1, 10, 4.8652, __QuantileHi2ValuesTestAccuracy, DisplayName = "p:0.1, k:10")]
        [DataRow(0.2, 10, 6.1791, __QuantileHi2ValuesTestAccuracy, DisplayName = "p:0.2, k:10")]
        [DataRow(0.3, 10, 7.2672, __QuantileHi2ValuesTestAccuracy, DisplayName = "p:0.3, k:10")]
        [DataRow(0.4, 10, 8.2955, __QuantileHi2ValuesTestAccuracy, DisplayName = "p:0.4, k:10")]
        [DataRow(0.5, 10, 9.3418, __QuantileHi2ValuesTestAccuracy, DisplayName = "p:0.5, k:10")]
        [DataRow(0.6, 10, 10.4732, __QuantileHi2ValuesTestAccuracy, DisplayName = "p:0.6, k:10")]
        [DataRow(0.7, 10, 11.7807, __QuantileHi2ValuesTestAccuracy, DisplayName = "p:0.7, k:10")]
        [DataRow(0.8, 10, 13.4420, __QuantileHi2ValuesTestAccuracy, DisplayName = "p:0.8, k:10")]
        [DataRow(0.9, 10, 15.9872, __QuantileHi2ValuesTestAccuracy, DisplayName = "p:0.9, k:10")]
        [DataRow(0.95, 10, 18.3070, __QuantileHi2ValuesTestAccuracy, DisplayName = "p:0.95, k:10")]
        public void QuantileHi2ValuesTest(double p, int k, double ExpectedValue, double Accuracy = 1e-16) =>
            Assert.That.Value(QuantileHi2(p, k)).IsEqual(ExpectedValue, Accuracy, $"Квантиль(p:{p}, k:{k})~{Accuracy}");

        [TestMethod, Ignore]
        public void QuantileHi2_n1_p001_v000015()
        {
            const int n = 1;
            const double p = 0.01;
            const double expected_value = 0.0001570878579097;
            var actual_value = QuantileHi2(p, n);
            Assert.That.Value(actual_value).IsEqual(expected_value);
        }

        [TestMethod, Ignore]
        public void QuantileHi2MatrixTest()
        {
            var nn = Enumerable.Range(1, 50).ToArray();
            double[] pp =
            {
                0.01, 0.05, 0.1, 0.15, 0.2, 0.25, 0.3, 0.35, 0.4, 0.45,
                0.5, 0.55, 0.6, 0.65, 0.7, 0.75, 0.8, 0.85, 0.9, 0.95, 0.995
            };

            var qchisq = new[,]
            {
               { 0.0001570878579097, 0.00393214000001952, 0.0157907740934313, 0.0357657791558978, 0.0641847546673015, 0.101531044267621, 0.148471861832546, 0.205900125227767, 0.274995897728455, 0.35731716828632, 0.454936423119573, 0.570651862051188, 0.708326300800795, 0.87345714298923, 1.07419417085759, 1.32330369693147, 1.64237441514982, 2.07225085582224, 2.70554345409541, 3.84145882069413, 7.87943857662246 },
               { 0.0201006717070029, 0.102586588775101,   0.210721031315653,  0.32503785899555,   0.446287102628419,  0.575364144903562, 0.713349887877465, 0.861565832184909, 1.02165124753198,  1.19567400151124, 1.38629436111989,  1.59701539243554,  1.83258146374831,  2.09964424899736, 2.40794560865187, 2.77258872223978, 3.2188758248682,  3.79423996977176, 4.60517018598809, 5.99146454710798, 10.5966347330961 },
               { 0.114831801899117,  0.351846317749272,   0.584374374155183,  0.797771444244722,  1.00517401305235,   1.21253290304567,  1.42365224303528,  1.64157559889882,  1.86916840338872,  2.10946650639279, 2.36597388437534,  2.64300526481825,  2.94616607310195,  3.2831124635255,  3.66487078317032, 4.10834493563232, 4.64162767608745, 5.3170478373171,  6.25138863117032, 7.81472790325118, 12.8381564665987 },
               { 0.297109480506531,  0.710723021397323,   1.06362321677922,   1.36647722614188,   1.64877661806596,   1.92255752622955,  2.19469842140697,  2.4700873874753,   2.75284268412576,  3.04694642427282, 3.3566939800333,   3.68713383408387,  4.04462649064933,  4.43768919938482, 4.87843296656042, 5.38526905777941, 5.98861669400426, 6.74488308721243, 7.77944033973487, 9.48772903678117, 14.8602590005602 },
               { 0.554298076728277,  1.14547622606177,    1.61030798696232,   1.99381634645305,   2.34253430584112,   2.67460280943216,  2.99990813275991,  3.32510739675921,  3.65549962314159,  3.99594445510145, 4.35146019109552,  4.72775658648385,  5.13186707440182,  5.57307000226777, 6.06442998415491, 6.62567976382925, 7.28927612664896, 8.11519941305293, 9.23635689978112, 11.0704976935164, 16.7496023436391 },
               { 0.872090330156585,  1.6353828943279,     2.20413065649864,   2.6612731761469,    3.07008840528928,   3.45459883572103,  3.82755158825412,  4.19726952767532,  4.57015380800675,  4.95187660633895, 5.34812062744711,  5.76519934076781,  6.21075719452671,  6.69476087605441, 7.23113533173199, 7.84080412058513, 8.55805972025068, 9.44610312678935, 10.6446406756684, 12.591587243744,  18.5475841785111 },
               { 1.23904230556793,   2.16734990929805,    2.83310691781534,   3.35828437920814,   3.82232190776613,   4.25485218354651,  4.67133044898107,  5.08164702963627,  5.49323486012309,  5.91252321545662, 6.3458111955215,   6.79997020902761,  7.28320763284032,  7.80612291559682, 8.3834308286084,  9.03714754790816, 9.80324990024085, 10.7478953328204, 12.0170366237805, 14.0671404493402, 20.2777398749626 },
               { 1.64649737269077,   2.73263679349966,    3.48953912564983,   4.07819909611163,   4.59357361205617,   5.07064042380019,  5.5274220852253,   5.97528912329896,  6.42264556024193,  6.87663106628113, 7.34412149770181,  7.83250903996953,  8.35052546775365,  8.90935886907783, 9.52445819307182, 10.2188549702467, 11.0300914303031, 12.0270737621362, 13.3615661365117, 15.5073130558654, 21.9549549906595 },
               { 2.08790073587073,   3.32511284306682,    4.16815900814611,   4.81652383831707,   5.3800532117733,    5.89882588296998,  6.39330596447532,  6.87626787078719,  7.35703450201812,  7.84341630949909, 8.34283269225297,  8.86316579421817,  9.41364009448283,  10.0059962378521, 10.656372006513,  11.3887514404704, 12.2421454698471, 13.2880400841358, 14.6836565732598, 16.9189776046205, 23.5893507812574 },
               { 2.55821216018721,   3.94029913611906,    4.86518205192534,   5.57005944421597,   6.1790792560394,    6.73720077195465,  7.26721816592762,  7.78324296829608,  8.2954717609411,   8.81235179862398, 9.34181776559199,  9.8922157257931,   10.4732362313954,  11.0971422819318, 11.780722627394,  12.5488613968894, 13.4419575749731, 14.533935995231,  15.9871791721052, 18.3070380532751, 25.1881795719712 },
               { 3.05348410664068,   4.57481307932223,    5.57778478979986,   6.33643471156884,   6.98867351223055,   7.5841427854413,   8.14786777750965,  8.69523839150006,  9.23728542384153,  9.78306345360455, 10.3409980743918,  10.9198769188551,  11.5298338409688,  12.1836284412343, 12.8986682017805, 13.7006927460115, 14.6314205088925, 15.7670952039679, 17.2750085175001, 19.6750593219595, 26.7568489164696 },
               { 3.5705689706044,    5.22602948839265,    6.30379605958433,   7.11383528464292,   7.80732767866101,   8.43841876613581,  9.03427658814019,  9.61151737996995,  10.1819713787516,  10.7552746382306, 11.3403223774242,  11.9463243526964,  12.5838379666175,  13.2660971251999, 14.0111001684219, 14.8454036710401, 15.8119862218969, 16.9893066811649, 18.5493477867032, 21.0260102758681, 28.299518822046  },
               { 4.10691547150441,   5.89186433770986,    7.04150458009548,   7.90083665392039,   8.63386083450622,   9.29906552985217,  9.92568241494693,  10.53150762508,    11.1291399404245,  11.7287740050926, 12.339755882564,   12.9717003427097,  13.6355709936619,  14.3450565017312, 15.1187216500487, 15.983906216312,  16.984797018243,  18.2019771889129, 19.8118953769661, 22.3619861547538, 29.8194712236532 },
               { 4.66042506265777,   6.57063138378934,    7.78953360975237,   8.69629634876529,   9.46732798687842,   10.1653138053771,  10.8214777216669,  11.4547540601933,  12.0784824788206,  12.7033957807038, 13.3392741490995,  13.9961221641787,  14.6852942562867,  15.4209165359312, 16.2220986133856, 17.1169335960001, 18.1507705624085, 19.4062364408489, 21.0641171435337, 23.6847545341105, 31.3193496225953 },
               { 5.22934888409896,   7.26094392767003,    8.54675624170455,   9.49928159654871,   10.3069590066253,   11.036537659091,   11.721168972945,   12.3808877556879,  13.0297495993745,  13.6790070877846, 14.3388595109567,  15.0196876364233,  15.7332229515878,  16.4940135311793, 17.3216944984992, 18.2450856024151, 19.3106571105909, 20.603007816412,  22.3071076309433, 24.9957604670825, 32.8013206457919 },
               { 5.81221247013497,   7.96164557237856,    9.31223635379601,   10.3090191795387,   11.152116471163,    11.912219697416,   12.6243487640597,  13.3096046790429,  13.9827363387065,  14.6554994655198, 15.3384988850016,  16.0424792553085,  16.779536709932,   17.5646273413883, 18.4178943922278, 19.3688602205845, 20.4650792937878, 21.7930433609005, 23.5418108665613, 26.2962033032107, 34.2671865378267 },
               { 6.40775977773893,   8.67176020467007,    10.0851863346193,   11.1248597059224,   12.0022657252674,   12.791926423832,   13.5306761398215,  14.2406507480559,  14.9372718031015,  15.6327830072921, 16.3381823773925,  17.0645672908903,  17.8243872629421,  18.6329937537469, 19.5110223531242, 20.4886762383915, 21.614560533896,  22.9770176069473, 24.7690203004787, 27.5870914721187, 35.7184656590046 },
               { 7.01491090117258,   9.39045508068897,    10.8649361165089,   11.9462515398972,   12.8569530964119,   13.6752903503983,  14.4398623422605,  15.1738110389046,  15.8932117219243,  16.6107821925416, 17.3379023687407,  18.0860121405924,  18.8679041212485,  19.6993135959085, 20.601354114108,  21.6048897957282, 22.7595376493237, 24.1554610424988, 25.9894104057797, 28.86928249763,   37.1564514566067 },
               { 7.63272964757146,   10.117013063859,     11.6509100321269,   12.7727213829133,   13.7157897062904,   14.5619967314202,  15.3516602626052,  16.1089018193421,  16.8504329724158,  17.5894328507177, 18.3376528967564,  19.1068661403727,  19.9101988556358,  20.7637595649482, 21.6891265830149, 22.7178067441999, 23.9004102446755, 25.3288437647492, 27.2035602360845, 30.1435128355676, 38.5822565549342 },
               { 8.2603983325464,    10.8508113941826,    12.4426092104501,   13.6038595449049,   14.5784392170705,   15.4517735390477,  16.2658564850128,  17.045764552577,   17.8088294731943,  18.5686799029396, 19.3374292294283,  20.1271749761579,  20.9513683777637,  21.826481436251,  22.7745450736464, 23.8276869219174, 25.0374996344387, 26.4975728918429, 28.4119713080786, 31.4104205300561, 39.9968463129386 },
               { 8.89719794207721,   11.5913052088207,    13.2395979753953,   14.4393085924875,   15.4446084037697,   16.3443837624788,  17.1822651839304,  17.9842613068988,  18.7683090500841,  19.5484756509539, 20.3372275635479,  21.1469787966556,  21.9914974909184,  22.8876100974974, 23.8577888955324, 24.9347725686073, 26.1710947260324, 27.6620037681918, 29.6150813948584, 32.6705626941504, 41.4010647714176 },
               { 9.54249233878507,   12.3380145787906,    14.041493189422,    15.2787544742137,   16.3140397951677,   17.2396194047591,  18.1007233731679,  18.9242711880501,  19.7287910067988,  20.5287784595084, 21.3370448076726,  22.1663131002802,  23.0306608992205,  23.9472607155193, 24.9390123258983, 26.0392611389089, 27.3014494697563, 28.8224487315712, 30.8132753175592, 33.9244291903561, 42.7956549993085 },
               { 10.1957155557458,   13.0905141881728,    14.8479557992677,   16.1219194931389,   17.1865058544475,   18.1372967411558,  19.0210871578489,  19.8656875288666,  20.6902042158372,  21.5095517268984, 22.3368784231843,  23.1852094491809,  24.0689248090134,  25.0055352530247, 26.018362133135,  27.1413325774277, 28.4287885028451, 29.9791840603134, 32.0068934967408, 35.1724534743733, 44.1812752499711 },
               { 10.8563614755323,   13.8484250271702,    15.6586840525129,   16.9685566777063,   18.0618043233875,   19.0372525295236,  19.9432287420387,  20.8084156484974,  21.6524855995392,  22.4907630701763, 23.3367263060896,  24.2036960494501,  25.1063482189283,  26.0625221125492, 27.0959586188358, 28.2411469889675, 29.5533116742365, 31.1324555418247, 33.1962388067975, 36.415021289454,  45.5585119365306 },
               { 11.5239753722493,   14.6114076394833,    16.4734079986734,   17.8184452254161,   18.9397544578979,   19.9393409490197,  20.8670340137766,  21.752371046135,   22.6155789080134,  23.472383672695,  24.3365866978843,  25.221798226671,   26.1429839693275,  27.1183075550152, 28.171912888647,  29.3388475689101, 30.6751977097648, 32.2824829825972, 34.3815821275489, 37.6524777101911, 46.9278901600807 },
               { 12.1981469235056,   15.3791565832618,    17.2918849897388,   18.6713867796374,   19.8201939548724,   20.8434311030755,  21.7924005769055,  22.6974779304852,  23.5794337259239,  24.4543877561,    25.3364581174773,  26.2395388188103,  27.1788778841841,  28.1729619442379, 29.2463248049109, 30.4345630000464, 31.7946072091867, 33.4294638955978, 35.5631668833664, 38.8851329032658, 48.2898823324568 },
               { 12.8785043931446,   16.1513958496641,    18.113895966896,    19.5272023606918,   20.7029764212862,   21.7494049644996,  22.7192361326321,  23.6436680116942,  24.5440046578084,  25.4367521489082, 26.3363393085915,  27.2569385032392,  28.214076540918,   29.2265512469911, 30.3192845158632, 31.5284094299021, 32.9116851180526, 34.5735765412297, 36.7412127866991, 40.1132668797188, 49.6449152989942 },
               { 13.5647097546188,   16.9278750444225,    18.9392423719175,   20.3857298160081,   21.5879692730251,   22.6571556706459,  23.6474571377283,  24.5908795006414,  25.5092506541859,  26.4194559309048, 27.3362291986898,  28.2740147947581,  29.2486169857839,  30.2791352683567, 31.3908737276629, 32.6204921148863, 34.0265627826302, 35.7149824551407, 37.9159189500289, 41.3371334464625, 50.9933762684994 },
               { 14.2564545762747,   17.7083661828246,    19.7677435594748,   21.2468216859558,   22.4750519775112,   23.5665860984102,  24.5769876836812,  25.5390562736711,  26.4751344497939,  27.4024801376662, 28.3361268665844,  29.2907875086265,  30.2825347015209,  31.3307684273584, 32.4611667701701, 33.7109067973635, 35.1393596707002, 36.8538285648625, 39.0874664917652, 42.5569635160107, 52.3356177859337 },
               { 14.9534565284555,   18.4926609819535,    20.5992346145854,   22.1103434054489,   23.3641145737901,   24.4776076648863,  25.5077585538803,  26.4881471705042,  27.441622091949,   28.3858075132211, 29.336030553173,   30.3072709011775,  31.3158621009676,  32.3815004115664, 33.5302314958938, 34.7997408667982, 36.2501848222832, 37.9902489741516, 40.2560207331439, 43.7729678973971, 53.6719619302406 },
               { 15.6554564016814,   19.2805685591293,    21.4335645003108,   22.9761717789489,   24.2550564183535,   25.3901393114882,  26.4397064254493,  27.4381054002546,  28.4086825419553,  29.3694223015895, 30.3359415896629,  31.3234793595213,  32.3486288857352,  33.4313767329593, 34.5981300429323, 35.8870743449397, 37.3591380803394, 39.1243664766294, 41.4217330619341, 44.9853396643429, 55.0027038800239 },
               { 16.3622155476658,   20.0719134645483,    22.2705944766442,   23.8441936797378,   25.1477851159954,   26.3041066383592,  27.3727731894861,  28.388888035868,   29.3762873361848,  30.3533093363855, 31.3358583037302,  32.3394261221625,  33.3808623528356,  34.4804392026376, 35.6649194862203, 36.9729807265597, 38.4663111408831, 40.2562938472659, 42.5847425226969, 46.1942561763974, 56.3281149597109 },
               { 17.0735136723294,   20.8665339907148,    23.1101967436072,   24.7143049344311,   26.0422156033962,   27.2194411627552,  28.3069053689225,  29.3404555814046,  30.3444102962529,  31.3374568978617, 32.3357801689908,  33.3551234028866,  34.4125876581901,  35.528726338402,  36.7306523964282, 38.0575277007999, 39.5717884538641, 41.3861349502926, 43.7451771810805, 47.3998808132489, 57.6484452558585 },
               { 17.7891469235469,   21.664280712552,     23.9522532708994,   25.5864093614843,   26.9382693595895,   28.1360796803683,  29.2420536173934,  30.2927715997307,  31.3130267222495,  32.3218519532016, 33.3357067221459,  34.3705824980431,  35.4438280441961,  36.5762737164355, 37.7953773220165, 39.1407777732069, 40.6756479999371, 42.5139856944514, 44.9031553001887, 48.6023644704397, 58.9639258755194 },
               { 18.5089262270249,   22.4650152208827,    24.7966547836925,   26.4604179384845,   27.8358737224822,   29.0539637126471,  30.178172285748,   31.2458023906149,  32.2821154670871,  33.3064832173742, 34.3356375538106,  35.385813879859,   36.4746050371588,  37.6231142761741, 38.859139206972,  40.2227888046642, 41.7779619633871, 43.6399348605228, 46.0587863597194, 49.801846855417,  60.274770904781  },
               { 19.2326758321541,   23.2686090188938,    25.6432998798511,   27.33624807776,     28.7349612945204,   29.9730390263702,  31.1152190453667,  32.1995162934402,  33.2516552204165,  34.291340239887,  35.3355723008969,  36.4008272779333,  37.5049386193183,  38.6692785857566, 39.9219797544042, 41.303614480393,  42.878797317687,  44.7640648213971, 47.2121719428156, 50.9984576155598, 61.5811791147572 },
               { 19.9602320364072,   24.0749425566799,    26.4920942583499,   28.2138229935683,   29.635469423691,    30.8932552142524,  32.0531545594387,  33.1538851634042,  34.2216267667102,  35.2764133207059, 36.3355106402518,  37.4156317506888,  38.5348473793528,  39.7147950741051, 40.9839377443341, 42.3833047197979, 43.9782163371561, 45.8864521712631, 48.3634065110254, 52.1923173240609, 62.8833354537411 },
               { 20.6914420622572,   24.8839043833356,    27.3429500422429,   29.0930711471043,   30.5373397485063,   31.8145653273673,  32.9919418927372,  34.1088814990107,  35.1920122189069,  36.2616934367767, 37.335452283316,   38.4302357482476,  39.5643486445556,  40.7596902346195, 42.0450493125326, 43.4619060360242, 45.076277045806,  47.0071682775479, 49.512578084153,  53.3835383449629, 64.181412357406  },
               { 21.4261630649459,   25.6953903995748,    28.1957851824004,   29.9739257579429,   31.4405177975895,   32.7369255517689,  33.9315475097001,  35.0644800831108,  36.162794893172,   37.2471721775579, 38.3353969716173,  39.4446471679519,  40.593458597339,   41.8039888046146, 43.1053481960872, 44.5394618525668, 46.173033612545,  48.1262797668906, 50.6597688388882, 54.5722255949729, 65.4755709034681 },
               { 22.1642612529752,   26.5093031966931,    29.0505229305455,   30.8563243724486,   32.3449526360589,   33.6602949229844,  34.8719391060699,  36.0206573447478,  37.1339591984513,  38.2328416882468, 39.3353444729504,  40.4588734035542,  41.6221923782743,  42.8477139239369, 44.1648659504201, 45.6160127830342, 47.2685367003717, 49.2438489545345, 51.8050556377659, 55.7584772167217, 66.7659618328038 },
               { 22.9056111060811,   27.3255514699942,    29.9070913719952,   31.7402084812414,   33.2505965521965,   34.5846349079855,  35.8130867554999,  36.9773912124554,  38.1054905387347,  39.2186946196071, 40.3352945781243,  41.4729213889322,  42.6505641775223,  43.8908872756372, 45.2236321417101, 46.6915968791773, 48.3628337759382, 50.3599342249857, 52.9485104981149, 56.9423851757338, 68.0527264554415 },
               { 23.6500946778262,   28.1440494966826,    30.7654230100453,   32.6255231790695,   34.157404778929,    35.5099098756633,  36.7549623845209,  37.9346609839544,  39.0773752262876,  40.2047240834804, 41.3352470981804,  42.486797637056,   43.6785873162097,  44.9335292111193, 46.2816745180427, 47.7662498514706, 49.4559693848441, 51.4745903705304, 54.090201009114,  58.1240357914147, 69.3359974569003 },
               { 24.3976009718974,   28.9647166697756,    31.625454395189,    33.512216861485,    35.0653351666107,   36.4360857986041,  37.6975396157634,  38.8924472100605,  40.0496004043791,  41.190923613214,  42.3352018620057,  43.5005082748202,  44.7062743190718,  45.9756588618045, 47.3390191620888, 48.8400052658698, 50.5479853971847, 52.5878688931753, 55.2301907038022, 59.3035102107555, 70.6158996179664 },
               { 25.1480253828245,   29.7874770808619,    32.4871257934005,   34.4002409535664,   35.974348308769,    37.363130844758,   38.6407936278369,  39.8507315909539,  41.0221539782681,  42.1772871283567, 43.335158714275,   44.5140590742638,  45.7336369794768,  47.0172942390477, 48.3956906276932, 49.9128947198148, 51.6389212271896, 53.699818274728,  56.3685393918519, 60.4808848321314, 71.8925504589991 },
               { 25.901269193178,    30.6122591455954,    33.3503808885667,   35.2895496727864,   36.884406770707,    38.2910150138342,  39.5847010297528,  40.8094968832383,  41.9950245533922,  43.1638089030717, 44.3351175136716,  45.5274554806248,  46.7606864177873,  48.0584523237792, 49.4517120623959, 50.9849480000923, 52.7288140302154, 54.8104842190311, 57.5053034580466, 61.6562316854743, 73.1660608182251 },
               { 26.6572391204409,   31.4389952666971,    34.2151665148698,   36.1800998161957,   37.7954753277621,   39.219709993025,   40.5292397480873,  41.7687268164477,  42.9682013798609,  44.1504835377996, 45.3350781313436,  46.5407026376121,  47.7874331338726,  49.099149147134,  50.507105317617,  52.056193224792,  53.817698879882,  55.9199098697798, 58.6405361306851, 62.8296187741846, 74.4365353721016 },
               { 27.4158469076902,   32.2676215299734,    35.081432522099,    37.0718505066607,   38.7075206903105,   40.1491890270204,  41.4743889253388,  42.7284060178538,  43.9416743024848,  45.137305933771,  46.3350404495595,  47.5538054102242,  48.8138870544731,  50.1393998631514, 51.5618910479898, 53.1266569712706, 54.9056089277444, 57.0281360068658, 59.7742877235356, 64.0011103833871, 75.7040731046947 },
               { 28.1770089530289,   33.0980774294863,    35.949131160143,    37.9647630859779,   39.6205113581385,   41.0794268006213,  42.4201288281521,  43.6885199445855,  44.9154337156795,  46.1242712700256, 47.335004360534,   48.5667684053987,  49.840057576019,   51.1792188144774, 52.6160888011187, 54.1963643917731, 55.9925755475595, 58.1352012237776, 60.9066058544565, 65.1707673584924, 76.9687677320446 },
               { 28.9406459733815,   33.9303059264203,    36.8182174326738,   38.8588009269889,   40.534417488607,    42.0103993324886,  43.3664407642625,  44.6490548222113,  45.8894705226765,  47.1113749826436, 48.3349697654001,  49.5795959907398,  50.8659536034261,  52.2186195918751, 53.6697170988681, 55.2653393181369, 57.0786284659259, 59.2411420882456, 62.0375356433764, 66.3386473574881, 78.2307080866901 },
               { 29.7066843653912,   34.7642520146741,    37.6886485819093,   39.7539292872695,   41.449210777049,    42.9420838787653,  44.3133070071682,  45.6099975890456,  46.8637760985458,  48.0986127459338, 49.3349365733052,  50.5922923115366,  51.8915835853195,  53.2576150882412, 54.7227935111364, 56.3336043568098, 58.1637958808365, 60.3459932880247, 63.1671198919653, 67.5048050799244, 79.489978466829  }

            };

            for (var i = 0; i < nn.Length; i++)
                for (var j = 0; j < pp.Length; j++)
                {
                    var n = nn[i];
                    var p = pp[j];
                    var expected_value = qchisq[i, j];
                    var actual_value = QuantileHi2(p, n);
                    const double accuracy = 1e-20;
                    Assert.That.Value(actual_value).IsEqual(expected_value, accuracy, $"p:{p};k:{n}");
                }
        }
    }
}