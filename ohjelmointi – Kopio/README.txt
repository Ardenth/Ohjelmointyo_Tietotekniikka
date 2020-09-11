
TODO:

korjaa XML tiedostot tyhjista (ja format)
- 6.9 loppuvaihe

XML tiedostot Unityyn
- 6.9 XMLParse toimii ohjelman alussa kaikkien XML tiedostojen muuttajana

UI character sheet
- 6.9 kehitetaan

(+ generate random character -button,
 joka aktivoi generoinnin ja sheet update
 ja sisaltaa pyynnon halutulle levelille)

Character tiedot (Character sheet) ja ne UI:n sisalle

Level up (UI update)

Random Gen (kayttaa random gen -button)

Random Gen painotus 

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