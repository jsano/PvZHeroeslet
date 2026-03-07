public class StrategyDecks
{
    
    public StrategyDecks()
    {
        UserAccounts.allDecks.Add("Peas", new DeckBuilder.Deck(0));
        UserAccounts.allDecks["Peas"].cards = new() {
            { AllCards.NameToID("Pea Pod"), 3 },
            { AllCards.NameToID("Torchwood"), 4 },
            { AllCards.NameToID("Fire Peashooter"), 2 },
            { AllCards.NameToID("Vitamin Z"), 2 },
            { AllCards.NameToID("Flourish"), 2 },
            { AllCards.NameToID("Grow-shroom"), 4 },
            { AllCards.NameToID("Repeater"), 2 },
            { AllCards.NameToID("The Podfather"), 4 },
            { AllCards.NameToID("Bananasaurus Rex"), 2 },
            { AllCards.NameToID("Skyshooter"), 2 },
            { AllCards.NameToID("Threepeater"), 4 },
            { AllCards.NameToID("Plant Food"), 1 },
            { AllCards.NameToID("Primal Peashooter"), 3 },
            { AllCards.NameToID("Split Pea"), 3 },
            { AllCards.NameToID("Gatling Pea"), 2 },
        };
        UserAccounts.allDecks.Add("Shrooms", new DeckBuilder.Deck(1));
        UserAccounts.allDecks["Shrooms"].cards = new() {
            { AllCards.NameToID("Poison Mushroom"), 2 },
            { AllCards.NameToID("Shroom for Two"), 4 },
            { AllCards.NameToID("Astro-shroom"), 4 },
            { AllCards.NameToID("Buff-shroom"), 4 },
            { AllCards.NameToID("Fume-shroom"), 3 },
            { AllCards.NameToID("Shelf Mushroom"), 4 },
            { AllCards.NameToID("Sun-shroom"), 2 },
            { AllCards.NameToID("Mushroom Ringleader"), 3 },
            { AllCards.NameToID("Punish-shroom"), 4 },
            { AllCards.NameToID("Mushroom Grotto"), 4 },
            { AllCards.NameToID("Cosmic Mushroom"), 4 },
            { AllCards.NameToID("Toadstool"), 2 },
        };
        UserAccounts.allDecks.Add("Teamwork", new DeckBuilder.Deck(2));
        UserAccounts.allDecks["Teamwork"].cards = new() {
            { AllCards.NameToID("Wall-nut"), 2 },
            { AllCards.NameToID("Sunflower"), 2 },
            { AllCards.NameToID("Potato Mine"), 2 },
            { AllCards.NameToID("Kernel-pult"), 2 },
            { AllCards.NameToID("Garlic"), 2 },
            { AllCards.NameToID("Gardening Gloves"), 2 },
            { AllCards.NameToID("Water Chestnut"), 2 },
            { AllCards.NameToID("Twin Sunflower"), 2 },
            { AllCards.NameToID("Sun-shroom"), 2 },
            { AllCards.NameToID("Mixed Nuts"), 4 },
            { AllCards.NameToID("Pea-nut"), 2 },
            { AllCards.NameToID("2nd Best Taco of All Time"), 1 },
            { AllCards.NameToID("Solar Winds"), 4 },
            { AllCards.NameToID("Primal Wall-nut"), 2 },
            { AllCards.NameToID("Red Stinger"), 2 },
            { AllCards.NameToID("Locust Swarm"), 2 },
            { AllCards.NameToID("Body-Gourd"), 2 },
            { AllCards.NameToID("Cob Cannon"), 3 },
        };
        UserAccounts.allDecks.Add("Science", new DeckBuilder.Deck(3));
        UserAccounts.allDecks["Science"].cards = new() {
            { AllCards.NameToID("Teleport"), 2 },
            { AllCards.NameToID("Neutron Imp"), 2 },
            { AllCards.NameToID("Interdimensional Zombie"), 3 },
            { AllCards.NameToID("Genetic Experiment"), 2 },
            { AllCards.NameToID("Zombology Teacher"), 3 },
            { AllCards.NameToID("Beam Me Up"), 3 },
            { AllCards.NameToID("Zombot Drone Engineer"), 3 },
            { AllCards.NameToID("Cosmic Scientist"), 3 },
            { AllCards.NameToID("Transformation Station"), 3 },
            { AllCards.NameToID("Kite Flyer"), 3 },
            { AllCards.NameToID("Electrician"), 3 },
            { AllCards.NameToID("Rocket Science"), 2 },
            { AllCards.NameToID("Moonwalker"), 2 },
            { AllCards.NameToID("Wormhole Gatekeeper"), 2 },
            { AllCards.NameToID("Mad Chemist"), 2 },
            { AllCards.NameToID("Gadget Scientist"), 2 },
        };
        UserAccounts.allDecks.Add("Hit Face", new DeckBuilder.Deck(12));
        UserAccounts.allDecks["Hit Face"].cards = new() {
            { AllCards.NameToID("Quickdraw Con Man"), 2 },
            { AllCards.NameToID("Zombie Chicken"), 2 },
            { AllCards.NameToID("Killer Whale"), 3 },
            { AllCards.NameToID("Sumo Wrestler"), 2 },
            { AllCards.NameToID("Terrify"), 2 },
            { AllCards.NameToID("Black Hole"), 4 },
            { AllCards.NameToID("Smoke Bomb"), 4 },
            { AllCards.NameToID("Zombot Aerostatic Gondola"), 2 },
            { AllCards.NameToID("Space Pirate"), 3 },
            { AllCards.NameToID("Backyard Bounce"), 2 },
            { AllCards.NameToID("Line Dancer"), 4 },
            { AllCards.NameToID("Pogo Bouncer"), 4 },
            { AllCards.NameToID("Mixed-up Gravedigger"), 2 },
            { AllCards.NameToID("Walrus Rider"), 2 },
            { AllCards.NameToID("Zombie High Diver"), 2 },
        };

        UserAccounts.Instance.SaveData();
    }

}
