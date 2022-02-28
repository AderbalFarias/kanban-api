# kanban-api

### Como rodar a Api

No arquivo **appsettings.json** existem algumas chaves que necessitam de seus valores:<br>
- `"JwtKey": "[Sua jwt chave]"` Substituir o conteúdo `"[Sua jwt chave]"` por qualquer string randômica
- `"JwtIssuer": "[Url da api]"` Substituir o conteúdo `"[Sua da api]"` pela a url dessa api no momento `"http://localhost:5000/"`
- `"JwtAudience": "[Url dde quem vai consumir]"` Substituir o conteúdo `"[Url de quem vai consumir]"` pela a url do front-end (ui) `"http://localhost:3000/"`
- `"JwtTokenTimeInMinutes": 30` Define em quanto tempo o token irá expirar
- `"path": "[Seu local para logs]"` Substituir o conteúdo `"[Seu local para logs]"` por o caminho para logs no servidor ou em sua maquina
- A aplicação possui cors e suas origens são definidas no seguinte array aonde devem ser substituido os valores `"[Url do front-end]"` e `[Url do back-end (swagger)]` por suas respectivas urls e outros origens podem serem adicionadas
```
 "CorsOrigin": [
    "[Url do front-end]",
    "[Url do back-end (swagger)]"
  ],
```
Nota: no arquivo **appsettings.Development.json** deixei os valores que estou usando, caso execute a aplicação localmente esse arquivo deve ser removido, ter os seus valores substituidos pelos acima ou apenas remova as chaves que estão no mesmo.<br>

A api possui swagger como podemos ver na imagem a seguir:<br>
![Swagger](/Docs/api-swagger.png)<br>

O swagger necessita de autentição para que os requests funcionem que pode obtido enviando um request para o endpoint login o qual retornará um jwt token o mesmo deve ser adicionado ao header clicando no botão **Authorize** irá aparecer uma caixa para digitar o valor o valor deve ser **Bearer token** token deve ser substituido pelo valor copiado do retorno obtido do requeste de login.<br>
![Swagger Authorize](/Docs/swagger-authorize.png)<br><br>
Clique em Authorize e em seguida close<br>
![Swagger Authorized](/Docs/swagger-authorized.png)<br><br>
Swagger estará pronto para efetuar os requests, nos endpoints adicionei uma documentação de como executar-los<br>
![Swagger Post](/Docs/swagger-post.png)<br><br>
Exemplo do log a seguir:<br>
![Api Log](/Docs/api-log.png)<br>

#### O server pode ser IIS Express ou o gerado pelo Kanban.Api:
![Kanban.Api](/Docs/server.png)
![IIS Express](/Docs/server2.png)<br>

#### Pontos a melhorar:
- No momento não estou verificando se o usuário existe na base de dados pois não criei um mecanismo para criação dos usuários
- Log criado apenas em arquivos
