using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    [SerializeField] private UnifyText uText;
    
    private List<string> textList = new List<string>(){
        "I haven't seen so many people jumping on mushrooms since Super Mario Bros. made it cool.",
        "This place is giving me major 'Friends' vibes. Where's my lobster?",
"I feel like I'm stuck in a 'Saved by the Bell' episode. Who's going to break out into a Zack Morris-style timeout?",
"This is like 'The Matrix', except instead of fighting agents, we're fighting a dial-up internet connection.",
"I haven't seen so many butterfly clips since the Spice Girls were popular.",
"I feel like I'm in a 'Jurassic Park' movie, except instead of dinosaurs, it's a sea of frosted tips.",
"I haven't seen so many fanny packs since the Fresh Prince moved to Bel-Air.",
"This place is like a 'Baywatch' set, except instead of swimsuits, everyone's wearing flannel shirts.",
"I feel like I'm in a 'Clueless' movie, except instead of Cher's computer program, we have Siri.",
"I haven't seen so many people in overalls since 'Full House' was on the air.",
"This is like 'Pac-Man', except instead of ghosts, we're being chased by the fashion police.",
"I feel like I'm at a 'Beavis and Butt-head' marathon, except instead of MTV, it's on Twitch.",
"I haven't seen so many people playing with Tamagotchis since the '90s.",
"This place is like a 'Power Rangers' episode, except instead of fighting monsters, we're fighting over who gets to use the landline phone.",
"I feel like I'm in a 'Terminator' movie, except instead of fighting robots, we're fighting AOL CDs.",
"I haven't seen so many pagers since 'Saved by the Bell' was on the air.",
"This is like 'Tetris', except instead of blocks, we're trying to fit into our old '90s clothes.",
"I feel like I'm in a 'Home Alone' movie, except instead of booby traps, it's just a bunch of VHS tapes lying around.",
"I haven't seen so many people wearing slap bracelets since the '90s.",
"This place is like a 'Beverly Hills, 90210' set, except instead of drama, we're just watching people struggle with dial-up internet.",
"I feel like I'm in a 'Wayne's World' movie, except instead of a public access TV show, it's a TikTok account.",
"I haven't seen so many people wearing bucket hats since the '90s.",
"This is like 'Street Fighter', except instead of fighting, we're just trying to get to our dial-up internet provider before it shuts down for the night.",
"I feel like I'm in a 'Home Improvement' episode, except instead of tools, we're just using Google for everything.",
"I haven't seen so many people wearing neon since the '90s.",
"This place is like a 'Seinfeld' set, except instead of nothing, we're complaining about slow internet speeds.",
"I feel like I'm in a 'GoldenEye 007' game, except instead of weapons, we're just using emojis.",
"I haven't seen so many people wearing scrunchies since the '90s.",
"This is like 'Donkey Kong Country', except instead of barrels, we're dodging our old yearbooks.",
            "I feel like I'm in a 'Fresh Prince of Bel-Air' episode, except instead of Uncle Phil's mansion, we're just stuck at home.",
"I haven't seen so many people wearing high-top sneakers since the '90s.",
"This place is like a 'Dawson's Creek' set, except instead of dramatic love triangles, we're just swiping left and right on dating apps.",
"I feel like I'm in a 'Super Mario Bros.' game, except instead of mushrooms, we're collecting hand sanitizer.",
"I haven't seen so many people wearing JNCO jeans since the '90s.",
"This is like 'Sonic the Hedgehog', except instead of rings, we're trying to find the last roll of toilet paper.",
"I feel like I'm in a 'Scream' movie, except instead of a ghostface killer, it's just people forgetting to mute their microphones on Zoom calls.",
"I haven't seen so many people wearing windbreakers since the '90s.",
"This place is like a 'Xena: Warrior Princess' set, except instead of battles, we're just fighting over the last slice of pizza.",
"I feel like I'm in a 'Mortal Kombat' game, except instead of fatalities, it's just people getting mad at each other on social media.",
"I haven't seen so many people wearing puka shell necklaces since the '90s.",
"This is like 'Final Fantasy VII', except instead of a quest to save the world, it's just a quest to find a decent internet connection.",
"I feel like I'm in a 'Frasier' episode, except instead of high-brow humor, it's just people sending each other memes on WhatsApp.",
"I haven't seen so many people wearing acid-washed jeans since the '90s.",
"This place is like a 'Buffy the Vampire Slayer' set, except instead of fighting demons, we're just trying to survive a pandemic.",
"I feel like I'm in a 'Simpsons' episode, except instead of Springfield, it's just a bunch of people on their phones in their own homes.",
"I haven't seen so many people wearing platform shoes since the '90s.",
"This is like 'Tony Hawk's Pro Skater', except instead of doing tricks, we're just trying not to spill our coffee on our laptops.",
"I feel like I'm in a 'Sex and the City' episode, except instead of talking about relationships, it's just people complaining about their wifi speeds.",
"I haven't seen so many people wearing bandanas since the '90s.",
"This place is like a 'Charmed' set, except instead of fighting evil, it's just people binge-watching Netflix on their couches.",
"I feel like I'm in a 'Grand Theft Auto' game, except instead of stealing cars, it's just people stealing their roommates' snacks.",
"I haven't seen so many people wearing visors since the '90s.",
"This is like 'The Legend of Zelda', except instead of collecting rupees, we're just trying to collect enough toilet paper to last us a week.",
"I feel like I'm in a 'Daria' episode, except instead of high school drama, it's just people having existential crises in their bedrooms.",
"I haven't seen so many people wearing tie-dye shirts since the '90s.",
    };

    private string currentText;
    [SerializeField] private float letterDelay = 0.1f;
    [SerializeField] private float pauseDelay = 1f;
    
    private void Start()
    {
        ShuffleTextList();
        StartCoroutine(DisplayText());
    }

    private void ShuffleTextList()
    {
        int n = textList.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            string value = textList[k];
            textList[k] = textList[n];
            textList[n] = value;
        }
    }

    private IEnumerator DisplayText()
    {
        while (textList.Count > 0)
        {
            currentText = textList[0];
            textList.RemoveAt(0);
            uText.Text = "";

            for (int j = 0; j < currentText.Length; j++)
            {
                uText.Text += currentText[j];
                yield return new WaitForSeconds(letterDelay);
            }

            yield return new WaitForSeconds(pauseDelay);
        }
    }
}