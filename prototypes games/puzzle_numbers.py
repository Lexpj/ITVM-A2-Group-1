from math import radians
from random import randint, seed, choices
import pygame
from pygame.locals import * 


class Number:

    def __init__(self,x,y,number):
        self.x, self.y = x,y
        self.hit = False
        self.radius = 50
        self.number = number
        self.circleColor = (255,0,0)

    def inRange(self,pos,mult=1):
        return True if ((pos[0]-self.x)**2 + (pos[1]-self.y)**2)**0.5 <= self.radius*mult else False


    def draw(self):
        
        if self.hit:
            pygame.draw.circle(App.screen, self.circleColor, [self.x,self.y], self.radius, 7)
        
        else:
            pygame.draw.circle(App.screen, self.circleColor, [self.x,self.y], self.radius, 4)


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
        App.amountNumbers = 6
        App.numbers = []
        

        self.mapSetup()



    def mapSetup(self):
        
        for i in range(App.amountNumbers):

            approve = False
            while not approve:
                pos = (randint(App.WIDTH//8,App.WIDTH//8 * 7),randint(App.HEIGHT//8,App.HEIGHT//8 * 7))
                approve = True

                for nr in App.numbers:
                    if nr.inRange(pos,mult=2):
                        approve = False
                        break
            
            App.numbers.append(Number(pos[0],pos[1],i+1))
        
        # Make first coloured
        App.numbers[0].circleColor = (255,255,0)

    def onClick(self,pos):
        number = None
        for nr in App.numbers:
            if nr.inRange(pos):
                number = nr.number
                break
        
        if number != None:
            if App.numbers[number-1].circleColor == (255,255,0):
                App.numbers[number-1].circleColor = (0,255,0)
                App.numbers[number-1].hit = True
                if number < App.amountNumbers:
                    App.numbers[number].circleColor = (255,255,0)
            elif App.numbers[number-1].circleColor == (255,0,0):
                App.numbers = []
                self.mapSetup()
                    



    def run(self):
        """Run the main event loop."""
        while App.running:
            for event in pygame.event.get():
                if event.type == QUIT:
                    App.running = False
                elif event.type == pygame.MOUSEBUTTONDOWN and event.button == 1:
                    self.onClick(event.pos)                    

            App.screen.fill(Color('gray'))

            # Draw tiles
            for nr in App.numbers:
                nr.draw()

            pygame.display.update()
            self.clock.tick(30)

        pygame.quit()

if __name__ == '__main__':
    App().run()