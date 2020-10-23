﻿using UnityEngine;
public class WordsDataBase
{

    public static string[] EasyWords = new string[] { "poder", "correr", "comer", "jogar", "conhecer", "trabalhar", "dever","chamar","minion","estudar","programar","ganhar","requerer","tomar",
    "imprimir","sentar","servir","enviar","apresentar","agradecer","adiantar","lutar","virar","acompanhar","magoar","negociar","quebrar","salvar","tambem","assim","pai","pessoa","mulher","homem","amor",
    "ferramenta","porta","espirito","marido","vida","noite","computador","inicio","fim","lua","time","grupo","pessoa","obama","putin","china","cor","visual","bola",
    "musica","ideia","espaco","louco","cultura","palavra","mundo","trabalho","azul","branco","alto","baixo","esquerda","direita","frente","cima","olho","boca",
    };

    public static string[] MediunWords = new string[] { "godzilla", "Real Madrid","Barcelona","Rapidamente","Anita","dia","wolverine", "dificuldade","hardware","sentimento","hulk","homem de ferro","capitao america","viuva negra","thor","homem formiga","homem aranha","diferente","facebook","instagram",
    "nome","alma","lista","country","jazz","rock","samba","brasileirao","vetor","perigoso","esclarecer","fone","atualizacao","incluso","historico","presente","material",
    "coca cola","pepsi","fruki",};

    public static string[] HardWords = new string[] { "ardiloso","belicoso", "balburdia","fugaz","emblematico","Alarido", "implicito","leptospirose","pachorrento","tergiversar","recondito","heliopatia","zesto","internacional","infundibiliforme","paralelepipedo","pervio","polografia","esputação","contramina","alcantil","colportor","execravel","expurgo","garantido","criatividade","desenvolvimento","inovativo","rapidamente","triangulo","significativamente","plataformas",
 "falacioso","ginecologista","elefante","fragmentado","microsoft","retangular","cilindrico","compativel","otimizado","ortopedico","manicure","invertebrado","protuberancia","insuportavel","individual","autoridade","informativo","entretenimento","comentarios","incorporado", };

    public static string[] FutbolWords = new string[] { "mourinho","sergio ramos","madrid","barca","real madrid","barcelona","juventus","ball","champions","league","milan",
    "uefa","juiz","player","cristiano","ronaldo","messi","pogba","benzema","suarez","neymar","puyol","ronaldinho","robinho","kaka","ancelotti","levante","manchester",
   "campo","World Cup","brazil","bayern munich","ibrahimovic","zlatan","benfica","fc porto","arsenal","sevilla","dortmound","chelsea","valencia","ajax","lazio",
    "fifa","monaco","sporting","villarreal","liverpool","celtic","america","guadalajara","galaxi","Brazuca","boca junior","river plate","pepe","xavi","xavi alonso","modric",
    "marcelo","casillas","buffon","de gea","gareth bale","james","pique","iniesta","busquets","van persie","Wayne Rooney","zidane","pele","maradona","raul", "rodinei", "cortez", "renato gaucho", "coudet", };

    public static string GetWord(DifficultMode mode)
    {
        string[] ar = null;
        switch (mode)
        {
            case DifficultMode.EASY:
                ar = EasyWords;
                break;
            case DifficultMode.MEDIUM:
                ar = MediunWords;
                break;
            case DifficultMode.HARD:
                ar = HardWords;
                break;
            case DifficultMode.FUTBOL:
                ar = FutbolWords;
                break;
            default:
                ar = MediunWords;
                break;
        }
        int max = Random.Range(1, ar.Length);
        int min = Random.Range(0, (max - 1));
        int r = Random.Range(min, max);
        return ar[r];
    }
}