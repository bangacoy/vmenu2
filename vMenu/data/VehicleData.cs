using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace vMenuClient
{
    public static class VehicleData
    {
        public struct VehicleColor
        {
            public readonly int id;
            public readonly string label;

            public VehicleColor(int id, string label)
            {
                if (label == "veh_color_taxi_yellow")
                {
                    if (CitizenFX.Core.Native.API.GetLabelText("veh_color_taxi_yellow") == "NULL")
                    {
                        CitizenFX.Core.Native.API.AddTextEntry("veh_color_taxi_yellow", $"Taxi {CitizenFX.Core.Native.API.GetLabelText("IEC_T20_2")}");
                    }
                }
                else if (label == "veh_color_off_white")
                {
                    if (CitizenFX.Core.Native.API.GetLabelText("veh_color_off_white") == "NULL")
                    {
                        CitizenFX.Core.Native.API.AddTextEntry("veh_color_off_white", "Off White");
                    }
                }
                else if (label == "VERY_DARK_BLUE")
                {
                    if (CitizenFX.Core.Native.API.GetLabelText("VERY_DARK_BLUE") == "NULL")
                    {
                        CitizenFX.Core.Native.API.AddTextEntry("VERY_DARK_BLUE", "Very Dark Blue");
                    }
                }

                this.label = label;
                this.id = id;
            }
        }

        public static readonly List<VehicleColor> ClassicColors = new List<VehicleColor>()
        {
            new VehicleColor(0, "BLACK"),
            new VehicleColor(1, "GRAPHITE"),
            new VehicleColor(2, "BLACK_STEEL"),
            new VehicleColor(3, "DARK_SILVER"),
            new VehicleColor(4, "SILVER"),
            new VehicleColor(5, "BLUE_SILVER"),
            new VehicleColor(6, "ROLLED_STEEL"),
            new VehicleColor(7, "SHADOW_SILVER"),
            new VehicleColor(8, "STONE_SILVER"),
            new VehicleColor(9, "MIDNIGHT_SILVER"),
            new VehicleColor(10, "CAST_IRON_SIL"),
            new VehicleColor(11, "ANTHR_BLACK"),

            new VehicleColor(27, "RED"),
            new VehicleColor(28, "TORINO_RED"),
            new VehicleColor(29, "FORMULA_RED"),
            new VehicleColor(30, "BLAZE_RED"),
            new VehicleColor(31, "GRACE_RED"),
            new VehicleColor(32, "GARNET_RED"),
            new VehicleColor(33, "SUNSET_RED"),
            new VehicleColor(34, "CABERNET_RED"),
            new VehicleColor(35, "CANDY_RED"),
            new VehicleColor(36, "SUNRISE_ORANGE"),
            new VehicleColor(37, "GOLD"),
            new VehicleColor(38, "ORANGE"),

            new VehicleColor(49, "DARK_GREEN"),
            new VehicleColor(50, "RACING_GREEN"),
            new VehicleColor(51, "SEA_GREEN"),
            new VehicleColor(52, "OLIVE_GREEN"),
            new VehicleColor(53, "BRIGHT_GREEN"),
            new VehicleColor(54, "PETROL_GREEN"),

            new VehicleColor(61, "GALAXY_BLUE"),
            new VehicleColor(62, "DARK_BLUE"),
            new VehicleColor(63, "SAXON_BLUE"),
            new VehicleColor(64, "BLUE"),
            new VehicleColor(65, "MARINER_BLUE"),
            new VehicleColor(66, "HARBOR_BLUE"),
            new VehicleColor(67, "DIAMOND_BLUE"),
            new VehicleColor(68, "SURF_BLUE"),
            new VehicleColor(69, "NAUTICAL_BLUE"),
            new VehicleColor(70, "ULTRA_BLUE"),
            new VehicleColor(71, "PURPLE"),
            new VehicleColor(72, "SPIN_PURPLE"),
            new VehicleColor(73, "RACING_BLUE"),
            new VehicleColor(74, "LIGHT_BLUE"),

            new VehicleColor(88, "YELLOW"),
            new VehicleColor(89, "RACE_YELLOW"),
            new VehicleColor(90, "BRONZE"),
            new VehicleColor(91, "FLUR_YELLOW"),
            new VehicleColor(92, "LIME_GREEN"),

            new VehicleColor(94, "UMBER_BROWN"),
            new VehicleColor(95, "CREEK_BROWN"),
            new VehicleColor(96, "CHOCOLATE_BROWN"),
            new VehicleColor(97, "MAPLE_BROWN"),
            new VehicleColor(98, "SADDLE_BROWN"),
            new VehicleColor(99, "STRAW_BROWN"),
            new VehicleColor(100, "MOSS_BROWN"),
            new VehicleColor(101, "BISON_BROWN"),
            new VehicleColor(102, "WOODBEECH_BROWN"),
            new VehicleColor(103, "BEECHWOOD_BROWN"),
            new VehicleColor(104, "SIENNA_BROWN"),
            new VehicleColor(105, "SANDY_BROWN"),
            new VehicleColor(106, "BLEECHED_BROWN"),
            new VehicleColor(107, "CREAM"),

            new VehicleColor(111, "WHITE"),
            new VehicleColor(112, "FROST_WHITE"),

            new VehicleColor(135, "HOT PINK"),
            new VehicleColor(136, "SALMON_PINK"),
            new VehicleColor(137, "PINK"),
            new VehicleColor(138, "BRIGHT_ORANGE"),

            new VehicleColor(141, "MIDNIGHT_BLUE"),
            new VehicleColor(142, "MIGHT_PURPLE"),
            new VehicleColor(143, "WINE_RED"),

            new VehicleColor(145, "BRIGHT_PURPLE"),
            new VehicleColor(146, "VERY_DARK_BLUE"),
            new VehicleColor(147, "BLACK_GRAPHITE"),

            new VehicleColor(150, "LAVA_RED"),
        };

        public static readonly List<VehicleColor> MatteColors = new List<VehicleColor>()
        {
            new VehicleColor(12, "BLACK"),
            new VehicleColor(13, "GREY"),
            new VehicleColor(14, "LIGHT_GREY"),

            new VehicleColor(39, "RED"),
            new VehicleColor(40, "DARK_RED"),
            new VehicleColor(41, "ORANGE"),
            new VehicleColor(42, "YELLOW"),

            new VehicleColor(55, "LIME_GREEN"),

            new VehicleColor(82, "DARK_BLUE"),
            new VehicleColor(83, "BLUE"),
            new VehicleColor(84, "MIDNIGHT_BLUE"),

            new VehicleColor(128, "GREEN"),

            new VehicleColor(148, "Purple"),
            new VehicleColor(149, "MIGHT_PURPLE"),

            new VehicleColor(151, "MATTE_FOR"),
            new VehicleColor(152, "MATTE_OD"),
            new VehicleColor(153, "MATTE_DIRT"),
            new VehicleColor(154, "MATTE_DESERT"),
            new VehicleColor(155, "MATTE_FOIL"),
        };

        public static readonly List<VehicleColor> MetalColors = new List<VehicleColor>()
        {
            new VehicleColor(117, "BR_STEEL"),
            new VehicleColor(118, "BR BLACK_STEEL"),
            new VehicleColor(119, "BR_ALUMINIUM"),

            new VehicleColor(158, "GOLD_P"),
            new VehicleColor(159, "GOLD_S"),
        };

        public static readonly List<VehicleColor> UtilColors = new List<VehicleColor>()
        {
            new VehicleColor(15, "BLACK"),
            new VehicleColor(16, "FMMC_COL1_1"),
            new VehicleColor(17, "DARK_SILVER"),
            new VehicleColor(18, "SILVER"),
            new VehicleColor(19, "BLACK_STEEL"),
            new VehicleColor(20, "SHADOW_SILVER"),

            new VehicleColor(43, "DARK_RED"),
            new VehicleColor(44, "RED"),
            new VehicleColor(45, "GARNET_RED"),

            new VehicleColor(56, "DARK_GREEN"),
            new VehicleColor(57, "GREEN"),

            new VehicleColor(75, "DARK_BLUE"),
            new VehicleColor(76, "MIDNIGHT_BLUE"),
            new VehicleColor(77, "SAXON_BLUE"),
            new VehicleColor(78, "NAUTICAL_BLUE"),
            new VehicleColor(79, "BLUE"),
            new VehicleColor(80, "FMMC_COL1_13"),
            new VehicleColor(81, "BRIGHT_PURPLE"),

            new VehicleColor(93, "STRAW_BROWN"),

            new VehicleColor(108, "UMBER_BROWN"),
            new VehicleColor(109, "MOSS_BROWN"),
            new VehicleColor(110, "SANDY_BROWN"),

            new VehicleColor(122, "veh_color_off_white"),

            new VehicleColor(125, "BRIGHT_GREEN"),

            new VehicleColor(127, "HARBOR_BLUE"),

            new VehicleColor(134, "FROST_WHITE"),

            new VehicleColor(139, "LIME_GREEN"),
            new VehicleColor(140, "ULTRA_BLUE"),

            new VehicleColor(144, "GREY"),

            new VehicleColor(157, "LIGHT_BLUE"),

            new VehicleColor(160, "YELLOW")
        };

        public static readonly List<VehicleColor> WornColors = new List<VehicleColor>()
        {
            new VehicleColor(21, "BLACK"),
            new VehicleColor(22, "GRAPHITE"),
            new VehicleColor(23, "LIGHT_GREY"),
            new VehicleColor(24, "SILVER"),
            new VehicleColor(25, "BLUE_SILVER"),
            new VehicleColor(26, "SHADOW_SILVER"),

            new VehicleColor(46, "RED"),
            new VehicleColor(47, "SALMON_PINK"),
            new VehicleColor(48, "DARK_RED"),

            new VehicleColor(58, "DARK_GREEN"),
            new VehicleColor(59, "GREEN"),
            new VehicleColor(60, "SEA_GREEN"),

            new VehicleColor(85, "DARK_BLUE"),
            new VehicleColor(86, "BLUE"),
            new VehicleColor(87, "LIGHT_BLUE"),

            new VehicleColor(113, "SANDY_BROWN"),
            new VehicleColor(114, "BISON_BROWN"),
            new VehicleColor(115, "CREEK_BROWN"),
            new VehicleColor(116, "BLEECHED_BROWN"),

            new VehicleColor(121, "veh_color_off_white"),

            new VehicleColor(123, "ORANGE"),
            new VehicleColor(124, "SUNRISE_ORANGE"),

            new VehicleColor(126, "veh_color_taxi_yellow"),

            new VehicleColor(129, "RACING_GREEN"),
            new VehicleColor(130, "ORANGE"),
            new VehicleColor(131, "WHITE"),
            new VehicleColor(132, "FROST_WHITE"),
            new VehicleColor(133, "OLIVE_GREEN"),
        };

        public static class Vehicles
        {
            #region Vehicle List Per Class

            #region Compacts
            public static List<string> Compacts { get; } = new List<string>()
            {
		"ISSI4",
		"ASBO",
		"BLISTA",
		"KANJO",
		"BRIOSO",
		"BRIOSO2",
		"CLUB",
		"DILETTANTE",
		"DILETTANTE2",
		"ISSI5",
		"ISSI2",
		"ISSI3",
		"ISSI6",
		"PANTO",
		"PRAIRIE",
		"RHAPSODY",
		"WEEVIL",
            };
            #endregion
            #region Sedans
            public static List<string> Sedans { get; } = new List<string>()
            {
		"ASEA",
		"ASEA2",
		"ASTEROPE",
		"CINQUEMILA",
		"COGNOSCENTI",
		"COGNOSCENTI2",
		"COG55",
		"COG552",
		"DEITY",
		"EMPEROR",
		"EMPEROR2",
		"EMPEROR3",
		"FUGITIVE",
		"GLENDALE",
		"GLENDALE2",
		"INGOT",
		"INTRUDER",
		"PREMIER",
		"PRIMO",
		"PRIMO2",
		"REGINA",
		"ROMERO",
		"SCHAFTER2",
		"SCHAFTER6",
		"SCHAFTER5",
		"STAFFORD",
		"STANIER",
		"STRATUM",
		"STRETCH",
		"SUPERD",
		"SURGE",
		"TAILGATER",
		"TAILGATER2",
		"LIMO2",
		"WARRENER",
		"WARRENER2",
		"WASHINGTON",
            };
            #endregion
            #region SUVs
            public static List<string> SUVs { get; } = new List<string>()
            {
		"ASTRON",
		"BALLER",
		"BALLER2",
		"BALLER3",
		"BALLER5",
		"BALLER4",
		"BALLER6",
		"BALLER7",
		"BJXL",
		"CAVALCADE",
		"CAVALCADE2",
		"CONTENDER",
		"DUBSTA",
		"DUBSTA2",
		"FQ2",
		"GRANGER",
		"GRANGER2",
		"GRESLEY",
		"HABANERO",
		"HUNTLEY",
		"IWAGEN",
		"JUBILEE",
		"LANDSTALKER",
		"LANDSTALKER2",
		"MESA",
		"MESA2",
		"NOVAK",
		"PATRIOT",
		"PATRIOT2",
		"RADI",
		"REBLA",
		"ROCOTO",
		"SEMINOLE",
		"SEMINOLE2",
		"SERRANO",
		"SQUADDIE",
		"TOROS",
		"XLS",
		"XLS2",
            };
            #endregion
            #region Coupes
            public static List<string> Coupes { get; } = new List<string>()
            {
		"COGCABRIO",
		"EXEMPLAR",
		"F620",
		"FELON",
		"FELON2",
		"JACKAL",
		"ORACLE",
		"ORACLE2",
		"PREVION",
		"SENTINEL2",
		"SENTINEL",
		"WINDSOR",
		"WINDSOR2",
		"ZION",
		"ZION2",
            };
            #endregion
            #region Muscle
            public static List<string> Muscle { get; } = new List<string>()
            {
		"DOMINATOR4",
		"IMPALER2",
		"IMPERATOR",
		"SLAMVAN4",
		"DUKES3",
		"BLADE",
		"BUCCANEER",
		"BUCCANEER2",
		"BUFFALO4",
		"STALION2",
		"CHINO",
		"CHINO2",
		"CLIQUE",
		"COQUETTE3",
		"DEVIANT",
		"DOMINATOR",
		"DOMINATOR7",
		"DOMINATOR8",
		"DOMINATOR3",
		"YOSEMITE2",
		"DUKES2",
		"DUKES",
		"ELLIE",
		"FACTION",
		"FACTION2",
		"FACTION3",
		"DOMINATOR5",
		"IMPALER3",
		"IMPERATOR2",
		"SLAMVAN5",
		"GAUNTLET",
		"GAUNTLET3",
		"GAUNTLET5",
		"GAUNTLET4",
		"HERMES",
		"HOTKNIFE",
		"HUSTLER",
		"IMPALER",
		"SLAMVAN2",
		"LURCHER",
		"MANANA2",
		"MOONBEAM",
		"MOONBEAM2",
		"DOMINATOR6",
		"IMPALER4",
		"IMPERATOR3",
		"SLAMVAN6",
		"NIGHTSHADE",
		"PEYOTE2",
		"PHOENIX",
		"PICADOR",
		"DOMINATOR2",
		"RATLOADER",
		"RATLOADER2",
		"GAUNTLET2",
		"RUINER",
		"RUINER3",
		"RUINER2",
		"SABREGT",
		"SABREGT2",
		"SLAMVAN",
		"SLAMVAN3",
		"STALION",
		"TAMPA",
		"TULIP",
		"VAMOS",
		"VIGERO",
		"VIRGO",
		"VIRGO3",
		"VIRGO2",
		"VOODOO",
		"VOODOO2",
		"TAMPA3",
		"YOSEMITE",
            };
            #endregion
            #region SportsClassics
            public static List<string> SportsClassics { get; } = new List<string>()
            {
		"Z190",
		"ARDENT",
		"CASCO",
		"CHEBUREK",
		"CHEETAH2",
		"COQUETTE2",
		"DELUXO",
		"DYNASTY",
		"FAGALOA",
		"BTYPE2",
		"GT500",
		"INFERNUS2",
		"JB700",
		"JB7002",
		"MAMBA",
		"MANANA",
		"MICHELLI",
		"MONROE",
		"NEBULA",
		"PEYOTE",
		"PEYOTE3",
		"PIGALLE",
		"RAPIDGT3",
		"RETINUE",
		"RETINUE2",
		"BTYPE",
		"BTYPE3",
		"SAVESTRA",
		"STINGER",
		"STINGERGT",
		"FELTZER3", 
		"STROMBERG",
		"SWINGER",
		"TOREADOR",
		"TORERO",
		"TORNADO",
		"TORNADO2", 
		"TORNADO3", 
		"TORNADO4", 
		"TORNADO5", 
		"TORNADO6", 
		"TURISMO2", 
		"VISERIS",
		"ZTYPE",
		"ZION3", 
            };
            #endregion
            #region Sports
            public static List<string> Sports { get; } = new List<string>()
            {
		"DRAFTER", 
		"NINEF",
		"NINEF2", 
		"ALPHA",
		"ZR380", 
		"BANSHEE",
		"BESTIAGTS",
		"BLISTA2", //Blista Compact 
		"BUFFALO",
		"BUFFALO2", //Buffalo S 
		"CALICO", //Calico GTF 
		"CARBONIZZARE",
		"COMET2", //Comet 
		"COMET3", //Comet Retro Custom 
		"COMET6", //Comet S2 
		"COMET7", //Comet S2 Cabrio 
		"COMET4", //Comet Safari 
		"COMET5", //Comet SR 
		"COQUETTE",
		"COQUETTE4", //Coquette D10 
		"CYPHER",
		"TAMPA2", //Drift Tampa 
		"ELEGY", //Elegy Retro Custom 
		"ELEGY2", //Elegy RH8 
		"EUROS",
		"FELTZER2", //Feltzer 
		"FLASHGT",
		"FUROREGT",
		"FUSILADE",
		"FUTO",
		"FUTO2", //Futo GTX 
		"ZR3802", //Future Shock ZR380 
		"GB200",
		"BLISTA3", //Go Go Monkey Blista 
		"GROWLER",
		"HOTRING",
		"IMORGON",
		"ISSI7", //Issi Sport 
		"ITALIGTO",
		"ITALIRSX",
		"JESTER",
		"JESTER2", //Jester (Racecar) 
		"JESTER3", //Jester Classic 
		"JESTER4", //Jester RR 
		"JUGULAR",
		"KHAMELION",
		"KOMODA",
		"KURUMA",
		"KURUMA2", //Kuruma (Armored) 
		"LOCUST",
		"LYNX",
		"MASSACRO",
		"MASSACRO2", //Massacro (Racecar) 
		"NEO",
		"NEON",
		"ZR3803", //Nightmare ZR380 
		"OMNIS",
		"PARAGON",
		"PARAGON2", //Paragon R (Armored) 
		"PARIAH",
		"PENUMBRA",
		"PENUMBRA2", //Penumbra FF 
		"RAIDEN",
		"RAPIDGT",
		"RAPIDGT2", //Rapid GT Cabrio 
		"RAPTOR",
		"REMUS",
		"REVOLTER",
		"RT3000",
		"RUSTON",
		"SCHAFTER4", //Schafter LWB 
		"SCHAFTER3", //Schafter V12 
		"SCHLAGEN",
		"SCHWARZER",
		"SENTINEL3", //Sentinel Classic 
		"SEVEN70",
		"SPECTER",
		"SPECTER2", //Specter Custom 
		"BUFFALO3", //Sprunk Buffalo 
		"STREITER",
		"SUGOI",
		"SULTAN",
		"SULTAN2", //Sultan Classic 
		"SULTAN3", //Sultan RS Classic 
		"SURANO",
		"TROPOS",
		"VSTR", //V-STR 
		"VECTRE",
		"VERLIERER2",
		"VETO", //Veto Classic 
		"VETO2", //Veto Modern 
		"ZR350",
            };
            #endregion
            #region Super
            public static List<string> Super { get; } = new List<string>()
            {
		"PFISTER811", //811 
		"ADDER",
		"AUTARCH",
		"BANSHEE2", //Banshee 900R 
		"BULLET",
		"CHAMPION",
		"CHEETAH",
		"CYCLONE",
		"DEVESTE",
		"EMERUS",
		"ENTITYXF",
		"ENTITY2", //Entity XXR 
		"SHEAVA", //ETR1 
		"FMJ",
		"FURIA",
		"GP1",
		"IGNUS",
		"INFERNUS",
		"ITALIGTB",
		"ITALIGTB2", //Itali GTB Custom 
		"KRIEGER",
		"OSIRIS",
		"NERO",
		"NERO2", //Nero Custom 
		"PENETRATOR",
		"LE7B", //RE-7B 
		"REAPER",
		"VOLTIC2", //Rocket Voltic 
		"S80",
		"SC1",
		"SCRAMJET",
		"SULTANRS",
		"T20",
		"TAIPAN",
		"TEMPESTA",
		"TEZERACT",
		"THRAX",
		"TIGON",
		"TURISMOR",
		"TYRANT",
		"TYRUS",
		"VACCA",
		"VAGNER",
		"VIGILANTE",
		"VISIONE",
		"VOLTIC",
		"PROTOTIPO", //X80 Proto 
		"XA21",
		"ZENO",
		"ZENTORNO",
		"ZORRUSSO",
            };
            #endregion
            #region Motorcycles
            public static List<string> Motorcycles { get; } = new List<string>()
            {
		"AKUMA",
		"DEATHBIKE", //Apocalypse Deathbike 
		"AVARUS",
		"BAGGER",
		"BATI",
		"BATI2", //Bati 801RR 
		"BF400",
		"CARBONRS",
		"CHIMERA",
		"CLIFFHANGER",
		"DAEMON", //Daemon Lost MC variant 
		"DAEMON2", //Daemon Bikers DLC variant 
		"DEFILER",
		"DIABLOUS",
		"DIABLOUS2", //Diabolus Custom 
		"DOUBLE",
		"ENDURO",
		"ESSKEY",
		"FAGGIO2",
		"FAGGIO3", //Faggio Mod 
		"FAGGIO", //Faggio Sport 
		"FCR",
		"FCR2", //FCR 1000 Custom 
		"DEATHBIKE2", //Future Shock Deathbike 
		"GARGOYLE",
		"HAKUCHOU",
		"HAKUCHOU2", //Hakuchou Drag 
		"HEXER",
		"INNOVATION",
		"LECTRO",
		"MANCHEZ",
		"MANCHEZ2", //Manchez Scout 
		"NEMESIS",
		"NIGHTBLADE",
		"DEATHBIKE3", //Nightmare Deathbike 
		"OPPRESSOR",
		"OPPRESSOR2", //Oppressor Mk II 
		"PCJ",
		"RROCKET", //Rampant Rocket 
		"RATBIKE",
		"REEVER",
		"RUFFIAN",
		"SANCHEZ", //Sanchez livery variant 
		"SANCHEZ2",
		"SANCTUS",
		"SHINOBI",
		"SHOTARO",
		"SOVEREIGN",
		"STRYDER",
		"THRUST",
		"VADER",
		"VINDICATOR",
		"VORTEX",
		"WOLFSBANE",
		"ZOMBIEA", //Zombie Bobber 
		"ZOMBIEB", //Zombie Chopper 
            };
            #endregion
            #region OffRoad
            public static List<string> OffRoad { get; } = new List<string>()
            {
		"BRUISER", //Apocalypse Bruiser 
		"BRUTUS", //Apocalypse Brutus 
		"MONSTER3", //Apocalypse Sasquatch 
		"BIFTA",
		"BLAZER",
		"BLAZER5", //Blazer Aqua 
		"BLAZER2", //Blazer Lifeguard 
		"BODHI2",
		"BRAWLER",
		"CARACARA",
		"CARACARA2", //Caracara 4x4 
		"TROPHYTRUCK2", //Desert Raid 
		"DUBSTA3", //Dubsta 6x6 
		"DUNE",
		"DUNE3", //Dune FAV 
		"DLOADER",
		"EVERON",
		"FREECRAWLER",
		"BRUISER2", //Future Shock Bruiser 
		"BRUTUS2", //Future Shock Brutus 
		"MONSTER4", //Future Shock Sasquatch 
		"HELLION",
		"BLAZER3", //Hot Rod Blazer 
		"BFINJECTION",
		"INSURGENT",
		"INSURGENT2", //Insurgent Pick-Up 
		"INSURGENT3", //Insurgent Pick-Up Custom 
		"KALAHARI",
		"KAMACHO",
		"MONSTER", //Liberator 
		"MARSHALL",
		"MENACER",
		"MESA3", //Merryweather Mesa 
		"BRUISER3", //Nightmare Bruiser 
		"BRUTUS3", //Nightmare Brutus 
		"MONSTER5", //Nightmare Sasquatch 
		"NIGHTSHARK",
		"OUTLAW",
		"PATRIOT3", //Patriot Mil-Spec 
		"DUNE4", //Ramp Buggy mission variant 
		"DUNE5", //Ramp Buggy 
		"RANCHERXL",
		"RANCHERXL2", //Rancher XL North Yankton variant 
		"RCBANDITO",
		"REBEL2",
		"RIATA",
		"REBEL", //Rusty Rebel 
		"SANDKING2", //Sandking SWB 
		"SANDKING", //Sandking XL 
		"DUNE2", //Space Docker 
		"BLAZER4", //Street Blazer 
		"TECHNICAL",
		"TECHNICAL2", //Technical Aqua 
		"TECHNICAL3", //Technical Custom 
		"TROPHYTRUCK",
		"VAGRANT",
		"VERUS",
		"WINKY",
		"YOSEMITE3", //Yosemite Rancher 
		"ZHABA",
            };
            #endregion
            #region Industrial
            public static List<string> Industrial { get; } = new List<string>()
            {
                "BULLDOZER",
                "CUTTER",
                "DUMP",
                "FLATBED",
                "GUARDIAN",
                "HANDLER",
                "MIXER",
                "MIXER2",
                "RUBBLE",
                "TIPTRUCK",
                "TIPTRUCK2",
            };
            #endregion
            #region Utility
            public static List<string> Utility { get; } = new List<string>()
            {
		"AIRBUS",
		"AIRTUG",
		"BRICKADE",
		"BULLDOZER",
		"BUS",
		"CABLECAR",
		"CADDY", //Caddy golf variant 
		"CADDY2", //Caddy civilian variant 
		"CADDY3", //Caddy bunker variant 
		"COACH", //Dashound 
		"DOCKTUG",
		"RALLYTRUCK", //Dune 
		"PBUS2", //Festival Bus 
		"TRACTOR2", //Fieldmaster 
		"TRACTOR3", //Fieldmaster North Yankton variant 
		"FORKLIFT",
		"MOWER",
		"RENTALBUS", //Rental Shuttle Bus 
		"RIPLEY",
		"SADLER",
		"SADLER2", //Sadler North Yankton variant 
		"SCRAP",
		"SLAMTRUCK",
		"TAXI",
		"TOURBUS",
		"TOWTRUCK2", //Tow Truck Slamvan variant 
		"TOWTRUCK", //Towtruck Yankee variant 
		"TRACTOR",
		"TRASH",
		"TRASH2", //Trashmaster heist variant 
		"UTILLITRUCK", //Utility Truck cherry picker variant 
		"UTILLITRUCK2", //Utility Truck flatbed variant 
		"UTILLITRUCK3", //Utility Truck pick-up variant 
		"WASTLNDR",
        	"ARMYTANKER",
		"ARMYTRAILER",
		"ARMYTRAILER2",
		"BALETRAILER",
		"BOATTRAILER",
		"DOCKTRAILER",
		"FREIGHTTRAILER",
		"GRAINTRAILER",
		"PROPTRAILER",
		"RAKETRAILER",
		"TANKER",
		"TANKER2",
		"TR2",
		"TR3",
		"TR4",
		"TRAILERLOGS",
		"TRAILERS",
		"TRAILERS2",
		"TRAILERS3",
		"TRAILERS4",
		"TRAILERLARGE",
		"TRAILERSMALL",
		"TRAILERSMALL2",
		"TRFLAT",
		"TVTRAILER",
            };
            #endregion
            #region Vans
            public static List<string> Vans { get; } = new List<string>()
            {
		"BOXVILLE5", //Armored Boxville 
		"BISON",
		"BISON2", //McGill-Olsen Bison 
		"BISON3", //Mighty Bush Bison 
		"BOBCATXL",
		"BOXVILLE", //LSDWP Boxville 
		"BOXVILLE2", //Go Postal Boxville 
		"BOXVILLE3", //Humane Labs Boxville 
		"BOXVILLE4", //PostOp Boxville 
		"BURRITO",
		"BURRITO2", //Burrito Bugstars variant 
		"BURRITO3", //Burrito civilian variant 
		"BURRITO4", //Burrito McGill-Olsen variant 
		"BURRITO5", //Burrito North Yankton variant 
		"CAMPER",
		"SPEEDO2", //Clown Van 
		"GBURRITO", //Gang Burrito Lost MC variant 
		"GBURRITO2", //Gang Burrito heist variant 
		"JOURNEY",
		"MINIVAN",
		"MINIVAN2", //Minivan Custom 
		"PARADISE",
		"PONY",
		"PONY2", //Pony Smoke on the Water variant 
		"RUMPO",
		"RUMPO2", //Rumpo Deludamol variant 
		"RUMPO3", //Rumpo Custom 
		"SPEEDO",
		"SPEEDO4", //Speedo Custom 
		"SURFER",
		"SURFER2", //Surfer beater variant 
		"TACO",
		"YOUGA",
		"YOUGA2", //Youga Classic 
		"YOUGA3", //Youga Classic 4x4 
		"YOUGA4", //Youga Custom 
		"BISON",
		"BISON2", //McGill-Olsen Bison 
		"BISON3", //Mighty Bush Bison 
		"CONTENDER",
		"DUBSTA3", //Dubsta 6x6 
		"GUARDIAN",
		"PICADOR",
		"SADLER",
		"SADLER2", //Sadler North Yankton variant 
		"SLAMVAN",
		"SLAMVAN3", //Slamvan Custom 
		"YOSEMITE",
            };
            #endregion
            #region Cycles
            public static List<string> Cycles { get; } = new List<string>()
            {
		"BMX",
		"CRUISER",
		"TRIBIKE2", //Endurex Race Bike 
		"FIXTER",
		"SCORCHER",
		"TRIBIKE3", //Tri-Cycles Race Bike 
		"TRIBIKE", //Whippet Race Bike 
            };
            #endregion
            #region Boats
            public static List<string> Boats { get; } = new List<string>()
            {
        "AVISA", //Kraken Avisa 
		"DINGHY",
		"DINGHY2", //Dinghy 2-seater variant 
		"DINGHY3", //Dinghy heist variant 
		"DINGHY4", //Dinghy yacht variant 
        "DINGHY5", //Dinghy weaponized variant 
		"JETMAX",
        "KOSATKA",
        "LONGFIN", //Shitzu Longfin 
		"SUBMERSIBLE2", //Kraken 
		"MARQUIS",
		"PREDATOR",
        "PATROLBOAT", //Kurtz 31 Patrol Boat 
		"SEASHARK",
		"SEASHARK2", //Lifeguard Seashark 
		"SEASHARK3", //Seashark yacht variant 
		"SPEEDER",
		"SPEEDER2", //Speeder yacht variant 
		"SQUALO",
		"SUBMERSIBLE",
		"SUNTRAP",
		"TORO",
		"TORO2", //Toro yacht variant 
		"TROPIC",
		"TROPIC2", //Tropic yacht variant 
		"TUG",
            };
            #endregion
            #region Helicopters
            public static List<string> Helicopters { get; } = new List<string>()
            {
		"AKULA",
		"ANNIHILATOR",
		"ANNIHILATOR2", //Annihilator Stealth 
		"BUZZARD2", //Buzzard 
		"BUZZARD", //Buzzard Attack Chopper 
		"CARGOBOB", //Military Cargobob 
		"CARGOBOB2", //Jetsam Cargobob 
		"CARGOBOB3", //Cargobob Trevor Philips Industries variant 
		"CARGOBOB4", //Cargobob Drop Zone variant 
		"HUNTER", //FH-1 Hunter 
		"FROGGER",
		"FROGGER2", //Frogger Trevor Philips Industries variant 
		"HAVOK",
		"MAVERICK",
		"POLMAV",
		"SAVAGE",
		"SEASPARROW",
		"SKYLIFT",
		"SEASPARROW2", //Sparrow 
		"SEASPARROW3", //Sparrow (prop variant) 
		"SUPERVOLITO",
		"SUPERVOLITO2", //SuperVolito Carbon 
		"SWIFT",
		"SWIFT2", //Swift Deluxe 
		"VALKYRIE",
		"VALKYRIE2", //Valkyrie MOD.0 
		"VOLATUS",
            };
            #endregion
            #region Planes
            public static List<string> Planes { get; } = new List<string>()
            {
		"ALPHAZ1",
		"BLIMP", //Atomic Blimp 
		"AVENGER",
		"AVENGER2", //Avenger with folded wings 
		"STRIKEFORCE", //B-11 Strikeforce 
		"BESRA",
		"BLIMP3",
		"CARGOPLANE",
		"CUBAN800",
		"DODO",
		"DUSTER",
		"HOWARD", //Howard NX-25 
		"HYDRA",
		"JET",
		"STARLING", //LF-22 Starling 
		"LUXOR",
		"LUXOR2", //Luxor Deluxe 
		"STUNT", //Mallard 
		"MAMMATUS",
		"MILJET",
		"MOGUL",
		"NIMBUS",
		"NOKOTA", //P-45 Nokota 
		"LAZER", //P-996 LAZER 
		"PYRO",
		"BOMBUSHKA", //RM-10 Bombushka 
		"ROGUE",
		"ALKONOST", //RO-86 Alkonost 
		"SEABREEZE",
		"SHAMAL",
		"TITAN",
		"TULA",
		"MICROLIGHT", //Ultralight 
		"MOLOTOK", //V-65 Molotok 
		"VELUM",
		"VELUM2", //Velum 5-Seater 
		"VESTRA",
		"VOLATOL",
		"BLIMP2", //Xero Blimp 
            };
            #endregion
            #region Service
            public static List<string> Service { get; } = new List<string>()
            {
                "AIRBUS",
                "BRICKADE",
                "BUS",
                "COACH",
                "PBUS2",
                "RALLYTRUCK",
                "RENTALBUS",
                "TAXI",
                "TOURBUS",
                "TRASH",
                "TRASH2",
                "WASTELANDER",
            };
            #endregion
            #region Emergency
            public static List<string> Emergency { get; } = new List<string>()
            {
		"SCARAB", //Apocalypse Scarab 
		"APC",
		"AMBULANCE",
		"BARRACKS",
		"BARRACKS2", //Barracks Semi 
		"BARRAGE",
		"CHERNOBOG",
		"CRUSADER",
		"FBI", //FIB Buffalo 
		"FBI2", //FIB Granger 
		"FIRETRUK",
		"HALFTRACK",
		"SCARAB2", //Future Shock Scarab 
		"LGUARD",
		"SCARAB3", //Nightmare Scarab 
		"PRANGER", //Park Ranger 
		"POLICEB", //Police Bike 
		"POLICE2", //Police Cruiser Buffalo 
		"POLICE", //Police Cruiser Stanier 
		"POLICE3", //Police Cruiser Interceptor 
		"POLICEOLD1", //Police Rancher 
		"RIOT", //Police Riot 
		"POLICEOLD2", //Police Roadcruiser 
		"POLICET", //Police Transporter 
		"PBUS", //Prison Bus 
		"MINITANK", //RC Tank 
		"RIOT2", //RCV 
		"RHINO",
		"SHERIFF", //Sheriff Cruiser 
		"SHERIFF2", //Sheriff SUV 
		"THRUSTER",
		"KHANJALI", //TM-02 Khanjali 
		"POLICE4", //Unmarked Cruiser 
		"VETIR",
            };
            #endregion
            #region Military
            public static List<string> Military { get; } = new List<string>()
            {
                "APC",
                "BARRACKS",
                "BARRACKS2",
                "BARRACKS3",
                "BARRAGE",
                "CHERNOBOG",
                "CRUSADER",
                "HALFTRACK",
                "KHANJALI",
                "MINITANK", // CASINO HEIST (MPHEIST3) DLC - Requires b2060
                "RHINO",
                "SCARAB",
                "SCARAB2",
                "SCARAB3",
                "THRUSTER", // Jetpack
                "TRAILERSMALL2", // Anti Aircraft Trailer
                "VETIR", // CAYO PERICO (MPHEIST4) DLC - Requires b2189
            };
            #endregion
            #region Commercial
            public static List<string> Commercial { get; } = new List<string>()
            {
		"CERBERUS", //Apocalypse Cerberus 
		"BENSON",
		"BIFF",
		"CUTTER",
		"HANDLER", //Dock Handler 
		"DUMP",
		"FLATBED",
		"CERBERUS2", //Future Shock Cerberus 
		"GUARDIAN",
		"HAULER",
		"HAULER2", //Hauler Custom 
		"MIXER",
		"MIXER2", //Mixer 8-wheel variant 
		"MULE",
		"MULE2", //Mule ramp door variant 
		"MULE3", //Mule heist variant 
		"MULE5", //Mule Contract DLC variant 
		"MULE4", //Mule Custom 
		"CERBERUS3", //Nightmare Cerberus 
		"PACKER",
		"PHANTOM",
		"PHANTOM3", //Phantom Custom 
		"PHANTOM2", //Phantom Wedge 
		"POUNDER",
		"POUNDER2", //Pounder Custom 
		"RUBBLE",
		"STOCKADE", //Securicar 
		"STOCKADE3", //Securicar North Yankton variant 
		"TERBYTE",
		"TIPTRUCK", //Tipper 4-wheel variant 
		"TIPTRUCK2", //Tipper 6-wheel variant 
            };
            #endregion
            #region Trains
            public static List<string> Trains { get; } = new List<string>()
            {
		"FREIGHT",
		"FREIGHTCAR",
		"FREIGHTCONT1",
		"FREIGHTCONT2",
		"FREIGHTGRAIN",
		"METROTRAIN",
		"TANKERCAR",
            };
            #endregion
            #region OpenWheel
            public static List<string> OpenWheel { get; } = new List<string>()
            {
                "FORMULA",
                "FORMULA2",
                "OPENWHEEL1", // SUMMER SPECIAL (MPSUM) DLC - Requires b2060
                "OPENWHEEL2", // SUMMER SPECIAL (MPSUM) DLC - Requires b2060
            };
            #endregion


            /*
            Compacts = 0,
            Sedans = 1,
            SUVs = 2,
            Coupes = 3,
            Muscle = 4,
            SportsClassics = 5,
            Sports = 6,
            Super = 7,
            Motorcycles = 8,
            OffRoad = 9,
            Industrial = 10,
            Utility = 11,
            Vans = 12,
            Cycles = 13,
            Boats = 14,
            Helicopters = 15,
            Planes = 16,
            Service = 17,
            Emergency = 18,
            Military = 19,
            Commercial = 20,
            Trains = 21
             */

            public static Dictionary<string, List<string>> VehicleClasses { get; } = new Dictionary<string, List<string>>()
            {
                [GetLabelText("VEH_CLASS_0")] = Compacts,
                [GetLabelText("VEH_CLASS_1")] = Sedans,
                [GetLabelText("VEH_CLASS_2")] = SUVs,
                [GetLabelText("VEH_CLASS_3")] = Coupes,
                [GetLabelText("VEH_CLASS_4")] = Muscle,
                [GetLabelText("VEH_CLASS_5")] = SportsClassics,
                [GetLabelText("VEH_CLASS_6")] = Sports,
                [GetLabelText("VEH_CLASS_7")] = Super,
                [GetLabelText("VEH_CLASS_8")] = Motorcycles,
                [GetLabelText("VEH_CLASS_9")] = OffRoad,
                [GetLabelText("VEH_CLASS_10")] = Industrial,
                [GetLabelText("VEH_CLASS_11")] = Utility,
                [GetLabelText("VEH_CLASS_12")] = Vans,
                [GetLabelText("VEH_CLASS_13")] = Cycles,
                [GetLabelText("VEH_CLASS_14")] = Boats,
                [GetLabelText("VEH_CLASS_15")] = Helicopters,
                [GetLabelText("VEH_CLASS_16")] = Planes,
                [GetLabelText("VEH_CLASS_17")] = Service,
                [GetLabelText("VEH_CLASS_18")] = Emergency,
                [GetLabelText("VEH_CLASS_19")] = Military,
                [GetLabelText("VEH_CLASS_20")] = Commercial,
                [GetLabelText("VEH_CLASS_21")] = Trains,
                [GetLabelText("VEH_CLASS_22")] = OpenWheel,
            };
            #endregion

            public static string[] GetAllVehicles()
            {
                List<string> vehs = new List<string>();
                foreach (var vc in VehicleClasses)
                {
                    foreach (var c in vc.Value)
                    {
                        vehs.Add(c);
                    }
                }
                return vehs.ToArray();
            }
        }
    }
}
