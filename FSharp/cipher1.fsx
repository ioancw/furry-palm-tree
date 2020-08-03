let cipherText = "BT JPX RMLX PCUV AMLX ICVJP IBTWXVR CI M LMT’R PMTN, MTN YVCJX CDXV MWMBTRJ JPX
  AMTNGXRJBAH UQCT JPX QGMRJXV CI JPX YMGG CI JPX HBTW’R QMGMAX; MTN JPX HBTW RMY
  JPX QMVJ CI JPX PMTN JPMJ YVCJX. JPXT JPX HBTW’R ACUTJXTMTAX YMR APMTWXN, MTN PBR
  JPCUWPJR JVCUFGXN PBL, RC JPMJ JPX SCBTJR CI PBR GCBTR YXVX GCCRXN, MTN PBR HTXXR
  RLCJX CTX MWMBTRJ MTCJPXV. JPX HBTW AVBXN MGCUN JC FVBTW BT JPX MRJVCGCWXVR, JPX
  APMGNXMTR, MTN JPX RCCJPRMEXVR. MTN JPX HBTW RQMHX, MTN RMBN JC JPX YBRX LXT CI
  FMFEGCT, YPCRCXDXV RPMGG VXMN JPBR YVBJBTW, MTN RPCY LX JPX BTJXVQVXJMJBCT
  JPXVXCI, RPMGG FX AGCJPXN YBJP RAM"

//a quick scan of the text, shows a lot of repeated JPX and MTN.  Could these
//be 'the' and 'and' respectively?

let englishLanguageLetterFrequencies =
    [ 'e'; 't'; 'a'; 'o'; 'i'; 'n'; 's'; 'r'; 'h'; 'd'; 'l'; 'u'; 'c'; 'm'; 'f'; 'y';
      'w'; 'g'; 'p'; 'b'; 'v'; 'k'; 'x'; 'q'; 'j'; 'z' ]

let alphabet = [ 'A' .. 'Z' ]

//now get letter frequencies from input cipher text
let frequencies =
    cipherText
    |> Seq.toList //can use regex here
    |> Seq.filter (fun x -> Seq.contains x alphabet) //assuming spaces weren't encrypted.
    |> Seq.groupBy id
    |> Seq.map (fun (l, ls) -> l, Seq.length ls)
    |> Seq.sortByDescending snd
    |> Seq.map fst

frequencies |> Seq.iter (fun c -> printfn "%c" c)

let lettersInCipherText = Seq.length frequencies
Seq.length englishLanguageLetterFrequencies

let key1 =
    Seq.zip frequencies englishLanguageLetterFrequencies

key1
|> Seq.iter (fun (c, p) -> printfn "('%c','%c')" c p)

key1
|> Seq.iter (fun (c, p) -> printfn "%c -> %c" c p)


let decipher (charMap: seq<char * char>) =
  charMap
  |> Seq.fold (fun (state: string) (c, p) -> state.Replace(c, p)) cipherText

key1 |> decipher

let key2 =
    [ ('X', 'e')
      ('J', 't')
      ('M', 'a')
      ('P', 'o')
      ('T', 'i')
      ('C', 'n')
      ('R', 's')
      ('B', 'r')
      ('V', 'h')
      ('N', 'd')
      ('G', 'l')
      ('W', 'u')
      ('A', 'c')
      ('Y', 'm')
      ('I', 'f')
      ('H', 'y')
      ('L', 'w')
      ('U', 'g')
      ('Q', 'p')
      ('F', 'b')
      ('D', 'v')
      ('E', 'k')
      ('S', 'x') ]

key2 |> decipher

//applying the frequency table:
//"ri toe sawe ongh cawe fnhto friuehs nf a wai���s oaid, aid mhnte nveh auarist toe caidlestrcy gpni toe plasteh nf toe mall nf toe yriu���s palace; aid toe yriu sam toe paht nf toe oaid toat mhnte. toei toe yriu���s cngiteiaice mas coaiued, aid ors tonguots thngbled orw, sn toat toe xnrits nf ors lnris mehe lnnsed, aid ors yiees swnte nie auarist aintoeh. toe yriu chred alngd tn bhriu ri toe asthnlnuehs, toe coaldeais, aid toe snntosakehs. aid toe yriu spaye, aid sard tn toe mrse wei nf babklni, monsneveh soall head tors mhrtriu, aid sonm we toe ritehphetatrni toehenf, soall be clntoed mrto sca"

//doesn't look like much
//maybe o is actually h
//so we need to change P to h and maybe V to o
let key3 =
    [ ('X', 'e')
      ('J', 't')
      ('M', 'a')
      ('P', 'h')
      ('T', 'i')
      ('C', 'n')
      ('R', 's')
      ('B', 'r')
      ('V', 'o')
      ('N', 'd')
      ('G', 'l')
      ('W', 'u')
      ('A', 'c')
      ('Y', 'm')
      ('I', 'f')
      ('H', 'y')
      ('L', 'w')
      ('U', 'g')
      ('Q', 'p')
      ('F', 'b')
      ('D', 'v')
      ('E', 'k')
      ('S', 'x') ]

key3 |> decipher
//"ri the sawe hngo cawe fnoth friueos nf a wai’s haid, aid monte nveo auarist the caidlestrcy gpni the plasteo nf the mall nf the yriu’s palace; aid the yriu sam the paot nf the haid that monte. thei the yriu’s cngiteiaice mas chaiued, aid hrs thnguhts tongbled hrw, sn that the xnrits nf hrs lnris meoe lnnsed, aid hrs yiees swnte nie auarist aintheo. the yriu cored alngd tn boriu ri the astonlnueos, the chaldeais, aid the snnthsakeos. aid the yriu spaye, aid sard tn the mrse wei nf babklni, mhnsneveo shall oead thrs mortriu, aid shnm we the riteopoetatrni theoenf, shall be clnthed mrth sca"

// at a guess sawe should be same and cawe should be came.
// so L is m and Y is w
// look to aid is possible and, so T is n and C is i (they are adjecent in frequencyies)

let key4 =
    [ ('X', 'e')
      ('J', 't')
      ('M', 'a')
      ('P', 'h')
      ('T', 'n')
      ('C', 'i')
      ('R', 's')
      ('B', 'r')
      ('V', 'o')
      ('N', 'd')
      ('G', 'l')
      ('W', 'u')
      ('A', 'c')
      ('Y', 'w')
      ('I', 'f')
      ('H', 'y')
      ('L', 'm')
      ('U', 'g')
      ('Q', 'p')
      ('F', 'b')
      ('D', 'v')
      ('E', 'k')
      ('S', 'x') ]

key4 |> decipher

//"rn the same higo came fioth frnueos if a man’s hand, and woite iveo auarnst the candlestrcy gpin the plasteo if the wall if the yrnu’s palace; and the yrnu saw the paot if the hand that woite. then the yrnu’s cigntenance was chanued, and hrs thiguhts toigbled hrm, si that the xirnts if hrs lirns weoe liised, and hrs ynees smite ine auarnst anitheo. the yrnu cored aligd ti bornu rn the astoiliueos, the chaldeans, and the siithsakeos. and the yrnu spaye, and sard ti the wrse men if babklin, whisieveo shall oead thrs wortrnu, and shiw me the rnteopoetatrin theoeif, shall be clithed wrth sca"
//starting to look a lot like english now.
// maybe fioth should be forth, looks somewhat biblical in nature.

let key5 =
    [ ('X', 'e')
      ('J', 't')
      ('M', 'a')
      ('P', 'h')
      ('T', 'n')
      ('C', 'o')
      ('R', 's')
      ('B', 'i')
      ('V', 'r')
      ('N', 'd')
      ('G', 'l')
      ('W', 'u')
      ('A', 'c')
      ('Y', 'w')
      ('I', 'f')
      ('H', 'y')
      ('L', 'm')
      ('U', 'g')
      ('Q', 'p')
      ('F', 'b')
      ('D', 'v')
      ('E', 'k')
      ('S', 'x') ]

key5 |> decipher
//"in the same hogr came forth finuers of a man’s hand, and wrote over auainst the candlesticy gpon the plaster of the wall of the yinu’s palace; and the yinu saw the part of the hand that wrote. then the yinu’s cogntenance was chanued, and his thoguhts trogbled him, so that the xoints of his loins were loosed, and his ynees smote one auainst another. the yinu cried alogd to brinu in the astrolouers, the chaldeans, and the soothsakers. and the yinu spaye, and said to the wise men of babklon, whosoever shall read this writinu, and show me the interpretation thereof, shall be clothed with sca"

//defo looking a lot more biblical in nature.
//hogr could be hour, go g should be u
//finuers should be fingers

let key6 =
    [ ('X', 'e')
      ('J', 't')
      ('M', 'a')
      ('P', 'h')
      ('T', 'n')
      ('C', 'o')
      ('R', 's')
      ('B', 'i')
      ('V', 'r')
      ('N', 'd')
      ('G', 'l')
      ('W', 'g')
      ('A', 'c')
      ('Y', 'w')
      ('I', 'f')
      ('H', 'y')
      ('L', 'm')
      ('U', 'u')
      ('Q', 'p')
      ('F', 'b')
      ('D', 'v')
      ('E', 'k')
      ('S', 'x') ]

key6 |> decipher

"in the same hour came forth fingers of a man’s hand, and wrote over against the candlesticy upon the plaster of the wall of the ying’s palace; and the ying saw the part of the hand that wrote. then the ying’s countenance was changed, and his thoughts troubled him, so that the xoints of his loins were loosed, and his ynees smote one against another. the ying cried aloud to bring in the astrologers, the chaldeans, and the soothsakers. and the ying spaye, and said to the wise men of babklon, whosoever shall read this writing, and show me the interpretation thereof, shall be clothed with sca"
//y should have been k

let key7 =
    [ ('X', 'e')
      ('J', 't')
      ('M', 'a')
      ('P', 'h')
      ('T', 'n')
      ('C', 'o')
      ('R', 's')
      ('B', 'i')
      ('V', 'r')
      ('N', 'd')
      ('G', 'l')
      ('W', 'g')
      ('A', 'c')
      ('Y', 'w')
      ('I', 'f')
      ('H', 'k')
      ('L', 'm')
      ('U', 'u')
      ('Q', 'p')
      ('F', 'b')
      ('D', 'v')
      ('E', 'y')
      ('S', 'j') ]

key7 |> decipher
//"in the same hour came forth fingers of a man���s hand, and wrote over against the candlestick upon the plaster of the wall of the king���s palace; and the king saw the part of the hand that wrote. then the king���s countenance was changed, and his thoughts troubled him, so that the xoints of his loins were loosed, and his knees smote one against another. the king cried aloud to bring in the astrologers, the chaldeans, and the soothsayers. and the king spake, and said to the wise men of babylon, whosoever shall read this writing, and show me the interpretation thereof, shall be clothed with sca"
