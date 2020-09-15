
TODO:

Overall design---

(+ generate random character -button,
 joka aktivoi generoinnin ja sheet update
 ja sisaltaa pyynnon halutulle levelille)

Character tiedot (Character sheet) ja ne UI:n sisalle

Random Gen (kayttaa random gen -button)

Random Gen painotus 

--
TODO:

Specific functionalities---

Skill Increase rajoitusta levelien perusteella (ei viela tarvetta)
- AddAdvancement metodi

Free boosts to prioritize relevant statistics (str, dex)
- Decided upon WHAT STAT CHARACTER GETS FROM CLASS FREE BONUS


Random var luonti metodissa
	- voidaan aina lisata 1 nanosekunin odotus identtisen estamiseksi
	- puhdistaa koodia


Monk class feats problem, Ki Spells / Ki Pool prerequisite not considered

--
---------
6.9:
UI:n kehitys Unityssa oli paljon haastavampaa kuin kuvittelin (talle projektille)

8.9:
paljon aikaa kaytetty "button" elementin toimintaa Unityssa
2 tunnin jalkeen tajusin etta on helpompaa suorittaa IPointerHandlerin kautta

https://answers.unity.com/questions/783279/46-ui-how-to-detect-mouse-over-on-button.html
https://forum.unity.com/threads/how-to-use-onpointerenter-event.294801/

jonka kautta hyodynsin menetelmaa IPointerClickHandler ja IPointerExitHandler
(neuvo tuli toiselta tietotekniikan opiskelijalta keskusteltaessa)

(harkintana onko IPointerExitHandler tarvetta, tuotin koska mahdollista kayttoa tulevaisuudessa)

Generate -button sailyy, silla se ei ole valilehtien vaihtoa varten

12.9:

XML files =>

Key | Element

Skill | Trained


-
| = definer for skill or feat (| or ||)
- = OR  (||||)
/ = AND (|||)

-------
DESIGN CHOICES
Advancement effect Shield Block (add feat)
- ei tarvetta siirtaa, silla advancement lasketaan featiksi
	- turhaan kaytetty aikaa tahan
ELI:
identtisia, se loytyy general featista ja advancementista

-------