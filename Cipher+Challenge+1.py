
# coding: utf-8

# In[70]:

cipher_text = "BT JPX RMLX PCUV AMLX ICVJP IBTWXVR CI M LMT’R PMTN, MTN YVCJX CDXV MWMBTRJ JPX AMTNGXRJBAH UQCT JPX QGMRJXV CI JPX YMGG CI JPX HBTW’R QMGMAX; MTN JPX HBTW RMY JPX QMVJ CI JPX PMTN JPMJ YVCJX. JPXT JPX HBTW’R ACUTJXTMTAX YMR APMTWXN, MTN PBR JPCUWPJR JVCUFGXN PBL, RC JPMJ JPX SCBTJR CI PBR GCBTR YXVX GCCRXN, MTN PBR HTXXR RLCJX CTX MWMBTRJ MTCJPXV. JPX HBTW AVBXN MGCUN JC FVBTW BT JPX MRJVCGCWXVR, JPX APMGNXMTR, MTN JPX RCCJPRMEXVR. MTN JPX HBTW RQMHX, MTN RMBN JC JPX YBRX LXT CI FMFEGCT, YPCRCXDXV RPMGG VXMN JPBR YVBJBTW, MTN RPCY LX JPX BTJXVQVXJMJBCT JPXVXCI, RPMGG FX AGCJPXN YBJP RAM"


# In[71]:

from collections import Counter
analysis = Counter(cipher_text)


# In[72]:

freq_analysis = analysis.most_common()
type(freq_analysis)
#list of tuples (character, count)


# In[73]:

#how to limit to alphabetic characters
import string
alphabet = []
for c in string.ascii_uppercase:
    alphabet.append(c)
alphabet

cipher_cleaned = []
for freq in freq_analysis:
    if freq[0] in alphabet:
        cipher_cleaned.append(freq)


# In[74]:

cipher_cleaned


# In[75]:

english_freq_table = ['e','t','a','o','i','n','s','r','h','d','l','u','c','m','f','y','w','g',
                      'p','b','v','k','x','q','j','z']


# In[76]:

combined = zip(cipher_cleaned,english_freq_table)


# In[77]:

plain_text = cipher_text
key = {}
for x, y in combined:
    print(x,y)
    key[x[0]] = y
    plain_text = plain_text.replace(x[0],y)


# In[78]:

plain_text


# In[79]:

key


# In[80]:

key['P'] = 'h'


# In[81]:

key['V'] = 'o'
key['B'] = 'i'
key['T'] = 'n'
key['C'] = 'r'
key['L'] = 'm'
key['Y'] = 'w'
key['C'] = 'o'
key['V'] = 'r'
key['W'] = 'g'
key['U'] = 'u'
key['H'] = 'k'
key['E'] = 'y'
key['S'] = 'j'


# In[82]:

plain_text2 = cipher_text
for ct in key:
    plain_text2 = plain_text2.replace(ct,key[ct])


# In[83]:

plain_text2


# In[ ]:



