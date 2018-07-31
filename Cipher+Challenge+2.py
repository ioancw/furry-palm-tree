
# coding: utf-8

# In[1]:

cipher_text = 'MHILY LZA ZBHL XBPZXBL MVYABUHL HWWPBZ JSHBKPBZ JHLJBZ KPJABT HYJHUBT LZA ULBAYVU'


# In[51]:

alphabet = list(string.ascii_lowercase)
shift_alphabet = list(string.ascii_uppercase)
print(cipher_text)
for shift in range(26):
    shift_alphabet = shift_alphabet[1:] + shift_alphabet[:1]
    key = zip(shift_alphabet, alphabet)
    plain_text = cipher_text
    for x, y in key:
        plain_text = plain_text.replace(x,y)
        
    print(plain_text)


# In[44]:

string.ascii_lowercase


# In[ ]:



