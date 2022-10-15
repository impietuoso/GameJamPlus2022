# GameJamPlus2022
### Método de desenvolvimento

- Cada branch será criada a partir da **MAIN** seguindo o padrão:
 ```
 [tipo_de_desenvolvimento]/[n_do_card_trello]/[rápida_descrição]
 ```
 Exemplo:
 ```
 feat/35/adicionar-logica-dialogo
 ```
 - Cada PR(Pull Request) será primeiramente criada para a branch **Develop** para realização de testes por outros.
 - Tendo sucesso nos testes em Develop. Será criada uma PR para uma branch de **release** criada a partir da **Main**.
 - Com a branch **release** testada e aprovada com todas as branchs de dev, será realizado o PR para a **MAIN** e a compilação do jogo.
 - A exceção do fluxo será em casos de Hot Fix (Correção de Bugs impeditivos diretamente na MAIN)
