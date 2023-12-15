using AdventOfCode2023.Helpers;

namespace AdventOfCode2023.Days;

public abstract class Day07
{
    public static int Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static int Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    public static int Part1(List<string> input)
    {
        var pokerHands = input.Select(GetPokerHandFromString).ToList();

        pokerHands.Sort(delegate (PokerHand first, PokerHand second)
        {
            return first.Compare(second);
        });

        return pokerHands.Select((elem, ind) => elem.Bid * (ind + 1)).Sum();
    }

    public static int Part2(List<string> input)
    {
        var pokerHands = input.Select(GetPokerHandFromStringPart2).ToList();

        pokerHands.Sort(delegate (PokerHand first, PokerHand second)
        {
            return first.ComparePart2(second);
        });

        return pokerHands.Select((elem, ind) => elem.Bid * (ind + 1)).Sum();
    }

    private static List<string> GetSeparatedInputFromFile() =>
        FileInputHelper.GetStringListFromFile("Day07.txt");

    private static PokerHand GetPokerHandFromString(string input)
    {
        var splitResult = input.Split(' ');
        var hand = splitResult[0];
        var bid = int.Parse(splitResult[1]);

        var dictOfFreq = new Dictionary<char, int>();
        foreach (var cardChar in hand)
        {
            if (!dictOfFreq.ContainsKey(cardChar)) dictOfFreq[cardChar] = 0;
            dictOfFreq[cardChar]++;
        }

        HandType handType;
        // 1. Check for five of a kind
        if (dictOfFreq.Values.Contains(5))
            handType = HandType.FiveOfAKind;
        else if (dictOfFreq.Values.Contains(4))
            handType = HandType.FourOfAKind;
        else if (dictOfFreq.Values.Contains(3))
        {
            if (dictOfFreq.Values.Contains(2))
                handType = HandType.FullHouse;
            else
                handType = HandType.ThreeOfAKind;
        }
        else if (dictOfFreq.Values.Contains(2))
        {
            // The Keys count will be 3 for two pair: one with 2, another with 2, and another with 1
            if (dictOfFreq.Keys.Count == 3)
                handType = HandType.TwoPair;
            else
                handType = HandType.OnePair;
        }
        else
            handType = HandType.HighCard;

        return new PokerHand
        {
            Hand = hand,
            Type = handType,
            Bid = bid,
        };
    }

    private static PokerHand GetPokerHandFromStringPart2(string input)
    {
        var splitResult = input.Split(' ');
        var hand = splitResult[0];
        var bid = int.Parse(splitResult[1]);

        var dictOfFreq = new Dictionary<char, int>();
        foreach (var cardChar in hand)
        {
            if (!dictOfFreq.ContainsKey(cardChar)) dictOfFreq[cardChar] = 0;
            dictOfFreq[cardChar]++;
        }

        var numOfJokers = 0;
        if (dictOfFreq.ContainsKey('J'))
            numOfJokers = dictOfFreq['J'];

        HandType handType;
        // 1. Check for five of a kind
        if (dictOfFreq.Values.Contains(5))
            // Cases:
            // JJJJJ
            // XXXXX
            handType = HandType.FiveOfAKind;
        else if (dictOfFreq.Values.Contains(4))
        {
            // Cases:
            // JJJJX
            // XXXXJ
            // XXXXY
            handType = numOfJokers > 0
                ? HandType.FiveOfAKind
                : HandType.FourOfAKind;
        }
            
        else if (dictOfFreq.Values.Contains(3))
        {
            if (dictOfFreq.Values.Contains(2))
            {
                if (numOfJokers > 0)
                    handType = HandType.FiveOfAKind;
                else
                    handType = HandType.FullHouse;
            }
            else
            {
                handType = numOfJokers > 0
                    ? HandType.FourOfAKind
                    : HandType.ThreeOfAKind;
            }
        }
        else if (dictOfFreq.Values.Contains(2))
        {
            // The Keys count will be 3 for two pair: one with 2, another with 2, and another with 1
            if (dictOfFreq.Keys.Count == 3)
            {
                // Cases:
                // JJXXY
                // XXYYJ
                // XXYYZ
                if (numOfJokers == 2)
                    handType = HandType.FourOfAKind;
                else if (numOfJokers == 1)
                    handType = HandType.FullHouse;
                else
                    handType = HandType.TwoPair;
            }
            else
            {
                // Cases:
                // JJXYZ
                // XXYZJ
                // XXYZW
                if (numOfJokers > 0)
                    handType = HandType.ThreeOfAKind;
                else
                    handType = HandType.OnePair;
            }
        }
        else
        {
            // Cases:
            // ABCDE
            // ABCDJ

            handType = (numOfJokers > 0)
                ? HandType.OnePair
                : HandType.HighCard;
        }

        return new PokerHand
        {
            Hand = hand,
            Type = handType,
            Bid = bid,
        };
    }

    private class PokerHand
    {
        public string Hand { get; set; }
        public HandType Type { get; set; }
        public int Bid { get; set; }

        public int Compare(PokerHand pokerHand)
        {
            if ((int)Type < (int)pokerHand.Type)
                return -1;
            if ((int)Type > (int)pokerHand.Type)
                return 1;

            for (int i = 0; i < 5; i++)
            {
                var compareCards = CompareCards(Hand[i], pokerHand.Hand[i]);
                if (compareCards != 0) return compareCards;
            }

            return 0;
        }

        public int ComparePart2(PokerHand pokerHand)
        {
            if ((int)Type < (int)pokerHand.Type)
                return -1;
            if ((int)Type > (int)pokerHand.Type)
                return 1;

            for (int i = 0; i < 5; i++)
            {
                var compareCards = CompareCardsPart2(Hand[i], pokerHand.Hand[i]);
                if (compareCards != 0) return compareCards;
            }

            return 0;
        }
    }

    private static int CompareCards(char firstCard, char secondCard)
    {
        CardRank firstCardRank = (CardRank)Enum.Parse(typeof(CardRank), $"C{firstCard}");
        CardRank secondCardRank = (CardRank)Enum.Parse(typeof(CardRank), $"C{secondCard}");

        // Confusingly, as CardRank is best to worst, we do this:
        return Math.Sign((int)secondCardRank - (int)firstCardRank);
    }

    private static int CompareCardsPart2(char firstCard, char secondCard)
    {
        CardRankPart2 firstCardRank = (CardRankPart2)Enum.Parse(typeof(CardRankPart2), $"C{firstCard}");
        CardRankPart2 secondCardRank = (CardRankPart2)Enum.Parse(typeof(CardRankPart2), $"C{secondCard}");

        // Confusingly, as CardRank is best to worst, we do this:
        return Math.Sign((int)secondCardRank - (int)firstCardRank);
    }

    private enum CardRank
    {
        // Best to worst
        CA, CK, CQ, CJ, CT, C9, C8, C7, C6, C5, C4, C3, C2,
    }

    private enum CardRankPart2
    {
        // Best to worst
        CA, CK, CQ, CT, C9, C8, C7, C6, C5, C4, C3, C2, CJ,
    }

    private enum HandType
    {
        // Worst to best
        HighCard = 0,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind,
    }
}
