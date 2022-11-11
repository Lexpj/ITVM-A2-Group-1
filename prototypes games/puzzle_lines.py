from math import radians
from random import randint, seed, choices
import pygame
from pygame.locals import * 


class Tile:

    def __init__(self,x,y,):
        self.x, self.y = x,y
        self.colorLine = (0,255,255)
        self.colorCircle = (0,230,230)
        self.circleSize = App.WIDTH//(max(App.boardHeight,App.boardWidth)**3)
        self.orientation = 0
        self.lines = [0,0,0,0]
        
    
    def update(self):
        pass
    
    def shuffle(self):
        if self.lines == [0,1,0,1] or self.lines == [1,0,1,0]:
            self.orientation = randint(0,1)
        elif self.lines == [1,1,1,1] or self.lines == [0,0,0,0]:
            self.orientation = 0
        else:
            self.orientation = randint(0,3)


    def rotate(self,rot):
        if self.lines == [0,1,0,1] or self.lines == [1,0,1,0]:
            self.orientation = (self.orientation + 2 + rot) % 2

        elif sum(self.lines) != 0 or sum(self.lines) != 4:
            self.orientation = (self.orientation + 4 + rot) % 4

    def draw(self):
        if sum(self.lines) == 0:
            return

        pygame.draw.rect(App.screen, self.colorLine, [self.x,self.y,App.WIDTH//App.boardWidth,App.HEIGHT//App.boardHeight],1)

        if (self.lines[(0+self.orientation)%4]): #rechts
            pygame.draw.line(App.screen, self.colorLine, [self.x+App.WIDTH//App.boardWidth // 2,self.y + App.HEIGHT//App.boardHeight // 2], [self.x+App.WIDTH//App.boardWidth,self.y + App.HEIGHT//App.boardHeight // 2],5)

        if (self.lines[(3+self.orientation)%4]): #onder
            pygame.draw.line(App.screen, self.colorLine, [self.x+App.WIDTH//App.boardWidth // 2,self.y + App.HEIGHT//App.boardHeight // 2], [self.x+App.WIDTH//App.boardWidth // 2,self.y + App.HEIGHT//App.boardHeight],5)

        if (self.lines[(2+self.orientation)%4]): #links
            pygame.draw.line(App.screen, self.colorLine, [self.x+App.WIDTH//App.boardWidth // 2,self.y + App.HEIGHT//App.boardHeight // 2], [self.x,self.y + App.HEIGHT//App.boardHeight // 2],5)

        if (self.lines[(1+self.orientation)%4]): #boven
            pygame.draw.line(App.screen, self.colorLine, [self.x+App.WIDTH//App.boardWidth // 2,self.y + App.HEIGHT//App.boardHeight // 2], [self.x+App.WIDTH//App.boardWidth // 2,self.y],5)

        # Circle
        pygame.draw.circle(App.screen, self.colorCircle, [self.x+App.WIDTH//App.boardWidth // 2,self.y + App.HEIGHT//App.boardHeight // 2],self.circleSize)


class App:

    def __init__(self):
        """Initialize pygame and the application."""
        pygame.init()
        flags = RESIZABLE
        App.WIDTH = 700
        App.HEIGHT = 700
        App.screen = pygame.display.set_mode((App.WIDTH, App.HEIGHT), flags)
        App.clock = pygame.time.Clock()
        App.running = True

        # For the puzzle itself
        App.tiles = []
        App.boardWidth = 3
        App.boardHeight = 3

        for i in range(App.boardHeight):
            App.tiles.append([])
            for j in range(App.boardWidth):
                App.tiles[i].append(Tile(j*App.WIDTH//App.boardWidth,i*App.HEIGHT//App.boardHeight))
        
        App.density = 0.7
        App.config = self.mapSetup()


    def mapSetup(self):
        w,h = (App.boardWidth*(App.boardHeight-1), (App.boardHeight*(App.boardWidth-1)))
        k = w*h
        App.config = choices([0,1],k=k,weights=[1-App.density,App.density])
        for i in range(App.boardHeight):
            for j in range(App.boardWidth-1):
                if App.config[j+(i*2)]:
                    App.tiles[i][j].lines[0] = 1
                    App.tiles[i][j+1].lines[2] = 1 
        for i in range(App.boardWidth):
            for j in range(App.boardHeight-1):
                if App.config[j+w-1+(i*2)]:
                    App.tiles[j][i].lines[3] = 1
                    App.tiles[j+1][i].lines[1] = 1 

        # Shuffle:
        while self.checkState():
            for i in range(App.boardHeight):
                for j in range(App.boardWidth):
                    App.tiles[i][j].shuffle()
        
    def checkState(self):
        w = (App.boardWidth*(App.boardHeight-1))
        
        found = True
        for i in range(App.boardHeight):
            for j in range(App.boardWidth-1):
                if App.tiles[i][j].lines[(0+App.tiles[i][j].orientation)%4] != App.tiles[i][j+1].lines[(2+App.tiles[i][j+1].orientation)%4]:
                    found = False
                    break
        for i in range(App.boardWidth):
            for j in range(App.boardHeight-1):
                if App.tiles[j][i].lines[(3+App.tiles[j][i].orientation)%4] != App.tiles[j+1][i].lines[(1+App.tiles[j+1][i].orientation)%4]:
                    found = False
                    break
        
        # Change color
        for i in range(App.boardHeight):
            for j in range(App.boardWidth):
                if found:
                    App.tiles[i][j].colorLine = (0,255,0)
                    App.tiles[i][j].colorCircle = (0,230,0)
                else:
                    App.tiles[i][j].colorLine = (0,255,255)
                    App.tiles[i][j].colorCircle = (0,230,230)

        return found


    def getTileIndex(self,x):
        return (x[0]//(App.WIDTH//App.boardWidth),x[1]//(App.HEIGHT//App.boardHeight))

    def run(self):
        """Run the main event loop."""
        while App.running:
            for event in pygame.event.get():
                if event.type == QUIT:
                    App.running = False
                elif event.type == pygame.MOUSEBUTTONDOWN and event.button == 1:
                    tile = self.getTileIndex(event.pos)
                    App.tiles[tile[1]][tile[0]].rotate(1)
                    self.checkState()
                elif event.type == pygame.MOUSEBUTTONDOWN and event.button == 3:
                    tile = self.getTileIndex(event.pos)
                    App.tiles[tile[1]][tile[0]].rotate(-1)
                    self.checkState()

            App.screen.fill(Color('gray'))

            # Draw tiles
            for i in range(App.boardHeight):
                for j in range(App.boardWidth):
                    App.tiles[i][j].draw()

            pygame.display.update()
            self.clock.tick(30)

        pygame.quit()

if __name__ == '__main__':
    App().run()