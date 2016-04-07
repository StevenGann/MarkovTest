# MarkovTest
## A simple implementation of a Markov chain text generator in C#

I discovered Markov chains only recently while investigating nonsensical user reviews I found on a couple websites. I looked for examples in C#, but couldn't find any that were very well documented or simple to follow.
After reading up on the topic on Wikipedia, I tossed together a simple implementation.

##What is a Markov Chain?

Named after mathematician Andrey Markov, a Markov Chain is a sort of finite state machine where transitions between states are chosen by a weighted random choice. Usually from historical data, you calculate the probability that you'll transition to a specific state next based solely on the current state.

##How is that implemented here?

For my implementation, I start by parsing an input document (Franz Kafka's Metamorphosis is included for testing) and making an index of every unique word. I then scan through the document a second time and count how often every unique word is followed by another unique word or terminating punctuation mark. I take that count and normalize it to a probability between 0 and 1.

To build a sentence, I pick a random word, roll a weighted random number generator to pick the next word based on the probabilities I calculated earlier, and repeat for the word after that.
Lastly, I pretty it up a bit by removing unpaired open parethesis, removing leading spaces on terminating punctuation, and capitalizing the first letter of the sentence.

##What's the result like?

I've done most of my testing with Franz Kafka's Metamorphosis, courtesy of Project Gutenberg. Here are some highlights:

- Pointing, then he had lost interest in tears.

- Beer, for anywhere near his stick in any thought he would sometimes catch the cheese, although he could but Gregor.

- When she would urge each other day now; bones from the beds flew at all doors but in return.

- Footsteps in my room, Mr.

##Am I having a stroke?

Probably not. None of those sentences make any sense at all because Markov chains do not account for any meaningful context. Each word is selected solely based on the previous word. This has interesting sides effects.

- Word frequency is preserved. Each unique word is used just as often as it was in the source document.

- Short or very common phrases are preserved. If certain words follow each other very often, they will have a very high probability of retaining that order in the output sentence.

As a result, it LOOKS LIKE text until you actually try to read and comprehend it. In this way, it is very similar to the traditional "Lorem ipsum" placeholder text.

##How can this be improved?

Currently there are two major weaknesses I still need to address.

1. The first word is picked purely at random. Preferably, the first word should be a word that starts a sentence in the source text.

2. Case is preserved for all words, not just proper nouns. This was done to preserve proper nouns, but as a side effect it preserves capitalization in words that start sentences as well. Non-capitalized instances of the same word are identified as seperate words. I need to add a check fornon-capitalized versions of words, and keep the non-capitalized version.
