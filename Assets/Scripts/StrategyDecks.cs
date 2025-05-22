public class StrategyDecks
{
    
    public StrategyDecks()
    {
        UserAccounts.allDecks.Add("Peas", new DeckBuilder.Deck(0));
        UserAccounts.allDecks["Peas"].cards = new() {
            { AllCards.NameToID("Pea Pod"), 3 },
            { AllCards.NameToID("Torchwood"), 4 },
            { AllCards.NameToID("Black-eyed Pea"), 3 },
            { AllCards.NameToID("Fire Peashooter"), 2 },
            { AllCards.NameToID("Snow Pea"), 4 },
            { AllCards.NameToID("Fertilize"), 2 },
            { AllCards.NameToID("Flourish"), 2 },
            { AllCards.NameToID("Grow-shroom"), 4 },
            { AllCards.NameToID("Repeater"), 2 },
            { AllCards.NameToID("The Podfather"), 4 },
            { AllCards.NameToID("Bananasaurus Rex"), 2 },
            { AllCards.NameToID("Skyshooter"), 2 },
            { AllCards.NameToID("Threepeater"), 4 },
            { AllCards.NameToID("Brainana"), 1 },
            { AllCards.NameToID("Plant Food"), 1 },
        };
        UserAccounts.allDecks.Add("Hit Face", new DeckBuilder.Deck(11));
        UserAccounts.allDecks["Hit Face"].cards = new() {
            { AllCards.NameToID("Chimney Sweep"), 2 },
            { AllCards.NameToID("Mini Ninja"), 4 },
            { AllCards.NameToID("Smoke Bomb"), 4 },
            { AllCards.NameToID("Lurch for Lunch"), 2 },
            //{ AllCards.NameToID("Toxic Waste Imp"), 3 },
            { AllCards.NameToID("Backyard Bounce"), 2 },
            //{ AllCards.NameToID("Line Dancing"), 4 },
            //{ AllCards.NameToID("Pogo"), 4 },
            { AllCards.NameToID("Mixed-up Gravedigger"), 2 },
            //{ AllCards.NameToID("Walrus Rider"), 2 },
            { AllCards.NameToID("Fun-dead Raiser"), 2 },
        };

        UserAccounts.Instance.SaveData();
    }

}
