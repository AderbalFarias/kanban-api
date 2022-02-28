# kanban-api

### Como rodar a Api

No arquivo **appsettings.json** existem algumas chaves que necessitam de seus valores:<br>
- `"JwtKey": "[Sua jwt chave]"` Substituir o conte�do `"[Sua jwt chave]"` por qualquer string rand�mica
- `"JwtIssuer": "[Url da api]"` Substituir o conte�do `"[Sua da api]"` pela a url dessa api no momento `"http://localhost:5000/"`
- `"JwtAudience": "[Url dde quem vai consumir]"` Substituir o conte�do `"[Url de quem vai consumir]"` pela a url do front-end (ui) `"http://localhost:3000/"`
- `"JwtTokenTimeInMinutes": 30` Define em quanto tempo o token ir� expirar
- `"path": "[Seu local para logs]"` Substituir o conte�do `"[Seu local para logs]"` por o caminho para logs no servidor ou em sua maquina
- A aplica��o possui cors e suas origens s�o definidas no seguinte array aonde devem ser substituido os valores `"[Url do front-end]"` e `[Url do back-end (swagger)]` por suas respectivas urls e outros origens podem serem adicionadas
```
 "CorsOrigin": [
    "[Url do front-end]",
    "[Url do back-end (swagger)]"
  ],
```
Nota: no arquivo **appsettings.Development.json** deixei os valores que estou usando, caso execute a aplica��o localmente esse arquivo deve ser removido, ter os seus valores substituidos pelos acima ou apenas remova as chaves que est�o no mesmo.<br>

A api possui swagger como podemos ver na imagem a seguir:<br>
![Swagger](/Docs/api-swagger.png)

O swagger necessita de autenti��o para que os requests funcionem que pode obtido enviando um request para o endpoint login o qual retornar� um jwt token o mesmo deve ser adicionado ao header clicando no bot�o **Authorize** ir� aparecer uma caixa para digitar o valor o valor deve ser **Bearer token** token deve ser substituido pelo valor copiado do retorno obtido do requeste de login.<br>
![Swagger Authorize](/Docs/swagger-authorize.png)<br>
Clique em Authorize e em seguida close<br>
![Swagger Authorized](/Docs/swagger-authorized.png)<br>
Swagger estar� pronto para efetuar os requests, nos endpoints adicionei uma documenta��o de como executar-los<br>
![Swagger Post](/Docs/swagger-post.png)
Exemplo do log a seguir:<br>
![Api Log](/Docs/api-log.png)

#### O server pode ser IIS Express ou o gerado pelo Kanban.Api:
![Kanban.Api](/Docs/server.png)
![IIS Express](/Docs/server2.png)<br>

#### Pontos a melhorar:
- No momento n�o estou verificando se o usu�rio existe na base de dados pois n�o criei um mecanismo para cria��o dos usu�rios
- Log criado apenas em arquivos