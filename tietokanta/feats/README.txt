06.08:
TODO: General feat vs Skill Feat

04.08 muutokset:
+ Vaatimukset maaritelty "general feat" - tietokannalla
+ formatting:
| splitter for stats
|| splitter for skills
|||splitter AND
|||| splitter OR
||||| splitter for feat

TODO general: feat splitter kyseenalainen, ei ehka toimi?

TODO class: split classes erillisiin tiedostoihin? formatointi parempi?
---------------------------------------------------------------------

Tietokannan hyodyntaminen perustuu prerequisite eli esivaatimus alueisiin.
Talle muuttujalle rakennetaan tulkki, jonka avulla on mahdollista maaritella useaa vaatimusta samanaikaisesti, ilman useita muuttujia, jotka kantaisivat null -arvoja.

yritetaan kontrolloida minimi- ja maksimioperaatioiden maaraa vaatimuksien listaamisessa.

muutos:

level requirement on vakituinen muuttuja, joten se kirjoitetaan erikseen.

Muuttujissa kaksi erillista tulkkia:
stat tulkki ja skill tulkki

General Featit saattavat sisaltaa molemmat, pitaako tulkit saada toimimaan yhdessa vai kahdella iteraatiolla?