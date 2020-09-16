
TODO:

Overall design---

(+ generate random character -button,
 joka aktivoi generoinnin ja sheet update
 ja sisaltaa pyynnon halutulle levelille)

Character tiedot (Character sheet) ja ne UI:n sisalle

Random Gen (kayttaa random gen -button)

Random Gen painotus
(ainoastaan stats atm)
(jaljella advancement painotus(voiko puurakennetta hyodyntaa?))
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



Ongelmia ja mahdolliset ideoinnit:
- suurin osa ongelmia suunnitteluvaiheessa, johon ei ollut selvaa
	ratkaisua ennen tekemista, koska ei ollut varmuutta 
	koodin ratkaisumenetelmista ja luokista.

16.9
----

Randomgen on pakko tehda luokaksi, ei ole mahdollista valttaa
system sleepilla, liikaa aikaa generointiin.
- Eri ongelma Initial featissa (Mietitaan silti muutosta)
- Random.Range() -unityn oma?
	- jos toimii, ratkaisee system.sleep ongelman

Skill Increase rajoitusta levelien perusteella (ei viela tarvetta)
- Ongelma melkein ainoastaan Rogue -classissa (paljon skill increase)
- Rajoitetaan randomgenia SkillIncrease vaiheessa, riippuen levelista.

Monk class feats problem, Ki Spells / Ki Pool prerequisite not considered
	- lisataan class feats Ki Focus/Pool -feat.
	- tarvetta bard, sorcerer?

Rogue, Cleric, Sorcerer(?) Initial feat ongelma.
	Cleric kaytannossa oma class? (Warpriest vs Cloistered)

- miten korjata tai ottaa huomioon?

- Cleric: Description tasolla, jos elementtien linkitys ei toimi

- Rogue: todennakoisesti helposti liitettavissa.
	- if primaryStat => 
		if (this.class = rogue && feats.contains(racketeer)) 
			this.primaryStat = Dexterity-Strength
				(eli Dexterity => Dexterity-Strength)

		if (this.class = rogue && feats.contains(scoundrel)) 
			this.primaryStat = Dexterity-Charisma
				(eli Dexterity => Dexterity-Charisma)
	- Huom, muuttaa myos proficiency Medium armor, miten tehdaan?
		- onko mahdollista pysyvasti linkittaa jonkin kirjaston
		  elementin arvo toiseen?
		- en loyda menetelmaa, saatetaan jattaa description tasolle


Deity valinta (Pitaisiko olla feat, koska kaikilla ei ole samaa efektia)
	- Tarkoittaa Champion/Cleric efektia aseisiin? Onko muuta?

Sorcerer Bloodline
	- pitaisi olla ratkaistavissa descriptionissa