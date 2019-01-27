![Logo of the project](https://raw.githubusercontent.com/jehna/readme-best-practices/master/sample-logo.png)

# Carrotmageddon

> Home is where your food at!

A game made at Finnish Game Jam / Global Game Jam 2019.

## Play now!

The game is playable in the browser at:

https://carrotmageddon.herokuapp.com/

## Developing

The game consists of a Unity project and node.js server. The server can be
started via:

```shell
cd source/server
yarn
yarn start
```

This installs the npm dependencies with Yarn package manager and states the
server.

### Deploying

To deploy you must do the following:

1. Build the webgl unity build to source/server/build
2. Deploy the project to Heroku:

```shell
cd source/server
git checkout -- build/index.html # the build overrides our index.html with unworkable version
yarn deploy
```

This pushes the current version of the game to Heroku's version control.

## Features

This game is about bunnies that try to get as much carrots as possible. The game
is:

- Mulitplayer
- Web-based
- Infinite

## Contributing

This game was a one-off Global Game Jam hackathin game, and no active
development is being done after the event. If you want to contirbute, ping with
an issue first!

## Licensing

This game has been licensed with [Creative Commons
Attribution-Noncommercial-Share Alike 3.0
license](https://creativecommons.org/licenses/by-nc-sa/3.0/).
