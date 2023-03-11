<h1 align="center"> Consultar CEP </h1>



## Índice 

[Índice](#índice)

[Descrição do Projeto](#descrição-do-projeto)

[Funcionalidades](#funcionalidades)

[Acesso ao Projeto](#acesso-ao-projeto)

[Criando a base MySQL](#criando-a-base)

[Tecnologias utilizadas](#tecnologias-utilizadas)


## Descrição do Projeto
  Projeto back-end para consulta de CEP, consumindo informações do webservice ViaCEP e armazenando em uma base de dados.

## Funcionalidades
-   Todos os end-points estão documentados via Swagger e são mostrados ao rodar o projeto.
-   Consultar CEP: consulta o cep inserido através do end-point
-   Salva o CEP: ao final da consulta, caso o cep não esteja na base de dados ele é salvo.
-   Buscar por Logradouro: busca os ceps salvos na base de dados pelo logradouro end-point
-   Buscar por uf: retorna uma lista de todos os ceps salvos na base ao consultar pela uf end-point
-   Buscar por uf paginada: retorna os ceps salvos na base de forma paginada ao consultar pela uf end-point


## Acesso ao projeto

Para acessar o projeto, basta fazer download desse [repositório](https://github.com/wesleyluz/Consultar_CEP_CSharp).

## 🛠️ Abrir e rodar o projeto
Esse é um projeto MVC C#, utilizando o Framework .NET e Entity,para acessar o projeto, basta abri-lo na sua IDE favorita.
Ao abrir o projeto, verifique o arquivo [ConsultarCEP/appsettings.json](https://github.com/wesleyluz/Consultar_CEP_CSharp/blob/main/ConsultarCEP/appsettings.json),
verificando se as conexões com o seu banco sql estão corretas.
Antes de rodar o projeto verifique Criando a base SQL abaixo.
Pronto agora é só rodar, basta executar e acessar os end-points no seu navegador ou no PostMan.

## Criando a base SQL

Crie a base de dados [CEP] no seu banco SQL e rode o projeto, as migrations estão configuradas para usar e criar o banco com esse nome e criar a tabela automaticamente.

## Tecnologias utilizadas
-   `Linguagem`:C# <link rel="stylesheet" href="https://cdn.jsdelivr.net/gh/devicons/devicon@v2.15.1/devicon.min.css" width="20" height="20"/>
-   `FrameWork`: .NET <link rel="stylesheet" href="https://cdn.jsdelivr.net/gh/devicons/devicon@v2.15.1/devicon.min.css" width="20" height="20"/> 
-   `Base de Dados`: SQL
-   `IDE`: Visual Studio 2022.
