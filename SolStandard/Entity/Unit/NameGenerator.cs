using System;
using System.Collections.Generic;

namespace SolStandard.Entity.Unit
{
    public static class NameGenerator
    {
        private const int MaxCachedNames = 50;
        private static List<string> _usedNames;

        private static List<string> UsedNames
        {
            get { return _usedNames ?? (_usedNames = new List<string>()); }
        }

        private enum NameType
        {
            Male,
            Female,
            Beast
        }

        public static void ClearNameHistory()
        {
            UsedNames.Clear();
        }

        public static string GenerateUnitName(Role role)
        {
            switch (role)
            {
                case Role.Silhouette:
                    return "?";
                case Role.Champion:
                    return GenerateName(NameType.Male);
                case Role.Archer:
                    return GenerateName(NameType.Beast);
                case Role.Mage:
                    return GenerateName(NameType.Male);
                case Role.Bard:
                    return GenerateName(NameType.Female);
                case Role.Lancer:
                    return GenerateName(NameType.Male);
                case Role.Pugilist:
                    return GenerateName(NameType.Female);
                case Role.Duelist:
                    return GenerateName(NameType.Male);
                case Role.Cleric:
                    return GenerateName(NameType.Female);
                case Role.Marauder:
                    return GenerateName(NameType.Male);
                case Role.Paladin:
                    return GenerateName(NameType.Female);
                case Role.Slime:
                    return GenerateName(NameType.Beast);
                case Role.Troll:
                    return GenerateName(NameType.Beast);
                case Role.Orc:
                    return GenerateName(NameType.Beast);
                case Role.Necromancer:
                    return GenerateName(NameType.Beast);
                case Role.Skeleton:
                    return GenerateName(NameType.Beast);
                case Role.Goblin:
                    return GenerateName(NameType.Beast);
                case Role.Rat:
                    return GenerateName(NameType.Beast);
                case Role.Bat:
                    return GenerateName(NameType.Beast);
                case Role.Spider:
                    return GenerateName(NameType.Beast);
                default:
                    throw new ArgumentOutOfRangeException("role", role, null);
            }
        }

        private static string GenerateName(NameType nameType)
        {
            List<string> nameList = FetchNameList(nameType);

            //Name generation should not break because too many units have spawned with generated names.
            //Clear the cache after a significant number of units have spawned.
            if (UsedNames.Count > MaxCachedNames) ClearNameHistory();

            while (true)
            {
                string generatedName = nameList[GameDriver.Random.Next(nameList.Count)];

                if (UsedNames.Contains(generatedName)) continue;

                UsedNames.Add(generatedName);
                return generatedName;
            }
        }

        private static List<string> FetchNameList(NameType nameType)
        {
            switch (nameType)
            {
                case NameType.Male:
                    return MaleNames;
                case NameType.Female:
                    return FemaleNames;
                case NameType.Beast:
                    return BeastNames;
                default:
                    throw new ArgumentOutOfRangeException("nameType", nameType, null);
            }
        }

        private static readonly List<string> MaleNames = new List<string>
        {
            "Aleksandr",
            "Andrew",
            "Anthony",
            "Arthur",
            "Aaron",
            "Albert",
            "Brandon",
            "Bart",
            "Brewer",
            "Carlos",
            "Chris",
            "Connor",
            "Darien",
            "Daniel",
            "David",
            "Dalton",
            "Erik",
            "Elon",
            "Ezekiel",
            "Fred",
            "Ford",
            "Fenwick",
            "Garth",
            "Gary",
            "George",
            "Henry",
            "Harold",
            "Hanlon",
            "Ian",
            "Idris",
            "John",
            "Jack",
            "Johann",
            "Ken",
            "Kurt",
            "Landon",
            "Lambert",
            "Lazarus",
            "Marty",
            "Matthew",
            "Markus",
            "Nathan",
            "Norman",
            "Nash",
            "Oscar",
            "Owen",
            "Oswald",
            "Percy",
            "Peter",
            "Paul",
            "Quinton",
            "Quaid",
            "Ronald",
            "Roy",
            "Ralph",
            "Sven",
            "Sedrick",
            "Siegfried",
            "Talbot",
            "Tarsus",
            "Tracy",
            "Uriel",
            "Ulysses",
            "Victor",
            "Vincent",
            "Vash",
            "Walter",
            "Wasco",
            "Xavier",
            "Yuri",
            "Ziegler",
            "Zachary",
        };

        private static readonly List<string> FemaleNames = new List<string>
        {
            "Alice",
            "Alara",
            "Bess",
            "Brooke",
            "Bianca",
            "Chloe",
            "Charlotte",
            "Cecilia",
            "Celeste",
            "Daria",
            "Danielle",
            "Danika",
            "Erika",
            "Erin",
            "Emily",
            "Francine",
            "Faith",
            "Gretel",
            "Gwen",
            "Harley",
            "Holly",
            "Ivy",
            "Isabella",
            "Jolene",
            "Julia",
            "Jenna",
            "Kathryn",
            "Kaitlyn",
            "Kassandra",
            "Liara",
            "Linda",
            "Maria",
            "Madison",
            "Morgan",
            "Nadia",
            "Naomi",
            "Nina",
            "Natalia",
            "Orianna",
            "Olivia",
            "Patricia",
            "Pearl",
            "Paige",
            "Quinn",
            "Resha",
            "Ruby",
            "River",
            "Robyn",
            "Sheila",
            "Sam",
            "Sarah",
            "Temperance",
            "Tammy",
            "Thalia",
            "Ursula",
            "Victoria",
            "Valerie",
            "Wendy",
            "Willow",
            "Xyla",
            "Yvonne",
            "Yuna",
            "Zoe",
            "Zyla",
        };

        private static readonly List<string> BeastNames = new List<string>
        {
            "Arnox",
            "Aaryx",
            "Axon",
            "Bortol",
            "Blostov",
            "Bornok",
            "Carna",
            "Colstok",
            "Druk-ha",
            "Darno'k",
            "Dvalna",
            "Eex",
            "Enlorn",
            "Farkov",
            "Fennik",
            "Grok",
            "Garn",
            "Galstus",
            "Harf",
            "Host'ai",
            "Iona",
            "Joka'sto",
            "Jaw",
            "Kinkil",
            "Klova",
            "Lenoi",
            "Mar",
            "Mon'a",
            "Norta",
            "Nalna",
            "Nuta",
            "Oort",
            "Olonobo",
            "Pa'ta'ro",
            "Pok",
            "Qorn",
            "Qorf",
            "Qil'a",
            "Ro-an",
            "Ra'al",
            "Sonowa",
            "Sin",
            "Sun-la",
            "Tok",
            "Tren",
            "Tik",
            "Toh-no",
            "Una'va",
            "Ushan",
            "Unahi",
            "Voi",
            "Velda",
            "Wonoa",
            "Wilnura",
            "Xox",
            "Xovoz",
            "Yor",
            "Yanna",
            "Zor",
            "Zel",
            "Zan",
        };
    }
}