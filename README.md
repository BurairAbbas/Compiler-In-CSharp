# Compiler-Construction-In-C#
Project of **compiler construction**. BSCS-504  
Compiler is created in the Windows Form Application C# which compile my own language.
## Feature:
#### Lexical Analyzer:
Compiler generate *Token* by analysis from **lexical Analyzer**. </br>Try to cover all the scenerio to break the Character in code and remove bug in Tokenization. **Its 95% complete.**  </br>
#### Syntax Analyer:
**Syntax Analyzer** is only cover DEC and Initialize and print statement in code and all the possible error related to these 3 operations. Syntax Analyzer is not completed yet other operation are left for example For_St, While_St, If_st etc. All the methods of Non-Terminal CFGs are implemented in Syntax Analyzer, its needs a lot of debugging due to index++ in code. I submitted my project so may be i will not complete it xD. **Its 30% complete**
<hr>

##### Language:
Compiler is based on [SURB Language](Language/). Most of the Syntax is same as C#.  
Language [First and Follow](CFGs/First%20and%20Follow%20set.docx) and [Selection Set](CFGs/Proving%20Grammer%20is%20LL(Selection%20Set).docx) are completed.
Syntax Analyzer is implementated form [Syntax Implementation File](CFGs/Syntax_Implementation_CFG.docx). The Non-Terminal which have 'NULL' as First set, implement the Selection Set according to that Non-Terminal.  
Please go thought the **Selection Set** may be due to my deadline i did a mistake.
**No Semantic Analyzer implementation**


