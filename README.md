# Client-Serveur du jeu "6 qui prend"



### Infos

Sous Windows
Client: c# WPF MVVM
Server: c++

### Utilisation

Ce qu'il est requis pour lancer notre projet:

- Récupérer le projet
- Client : Lancer le binaire (bin/debug) ou via visual (en lancer plusieurs pour simuler les clients)
- Serveur: Lancer via visual car notre binaire(Server.exe) ne s'ouvre pas sorry ^^'

Normalement il y a pas d'argument a changer si vous lancer bien en localhost

## Régles implémentées du 6 qui prend

Joueur possible de 1 a 10
Le serveur supporte qu'une partie a la fois , si un joueur quitte la partie continue sans lui.
Une fois que le premier joueur ai rejoins la partie, le serveur lance un timer de 20sec avant de commencer.

- Le joueur peux envoyer une carte.
- Si il a la plus petite carte sur le jeu, il peux envoyer une ligne.
- Update visuel du board pour voir l'avancement du jeu et du score.
- Fin des 10 rounds on affiche le Score

## Auteurs
Julien Cottier et Henrico

