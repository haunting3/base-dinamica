:-dynamic poder/2.

comp(X,Y):-poder(X, Poderx), poder(Y, Podery),
    Poderx > Podery.
adiciona(X,Y):-asserta(poder(X,Y)).
remove(X):-retract(poder(X,_)).
carrega:-exists_file('banco.db'),
    consult('banco.db').


salva:-tell('banco.db'),
   listing(poder),
    told.
