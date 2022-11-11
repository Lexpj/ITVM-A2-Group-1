from math import radians
from random import randint, seed
import pygame
from pygame.locals import * 
seed(69)

class Disk:

    def __init__(self,size,speed,cone,neg=False):
        self.size = size
        self.speed = speed
        self.cone = cone
        self.spinning = True
        self.color = (randint(0,255),randint(0,255),randint(0,255))
        self.colorarc = (255,255,0)
        self.currad = 0
        self.neg = neg

    def update(self):
        if self.spinning:
            if not self.neg:
                self.currad += self.speed
            else:
                self.currad -= self.speed

    def draw(self):
        pygame.draw.circle(App.screen,self.color,[App.WIDTH//2,App.HEIGHT//2],self.size)
        pygame.draw.arc(App.screen, self.colorarc, [(App.WIDTH - 2*self.size)//2,(App.WIDTH - 2*self.size)//2, 2*self.size, 2*self.size], radians(self.currad), radians(self.currad+self.cone), self.size)


class App:

    def __init__(self):
        """Initialize pygame and the application."""
        pygame.init()
        flags = RESIZABLE
        App.WIDTH = 700
        App.HEIGHT = 700
        App.screen = pygame.display.set_mode((App.WIDTH, App.HEIGHT), flags)
        App.clock = pygame.time.Clock()

        App.disks = [Disk(size=300,speed=2,cone=60,neg=True),
                     Disk(size=250,speed=5,cone=50),
                     Disk(size=200,speed=8,cone=40,neg=True),
                     Disk(size=150,speed=12,cone=30),
                     Disk(size=100,speed=15,cone=20,neg=True)]
        
        App.running = True
        App.lines = [0]*App.disks[-1].cone

    def partialWin(self):
        """
        Draws keylock gain
        """
        curdisk = 0
        for i in range(len(App.disks)):
            if App.disks[i].spinning:
                break
            curdisk += 1


        for i in range(App.disks[-1].cone):
            length = 0
            while App.screen.get_at((length, App.HEIGHT//2 - App.disks[-1].cone//2 + i)) in [(255,255,0,255),(0,0,0,255)] \
                and ((curdisk == len(App.disks) and length <= App.WIDTH//2) or (length <= App.WIDTH//2-App.disks[curdisk].size)):
                length += 1
            App.lines[i] = length

    def run(self):
        """Run the main event loop."""
        while App.running:
            for event in pygame.event.get():
                if event.type == QUIT:
                    App.running = False
                elif event.type == pygame.KEYDOWN:
                    if event.key == pygame.K_SPACE:

                        found = False
                        for i in range(len(App.disks)):
                            if App.disks[i].spinning:
                                App.disks[i].spinning = False
                                found = True
                                self.partialWin()
                                break
                        
                        if not found:
                            #self.evalWin()
                            pass

            App.screen.fill(Color('gray'))

            # Update
            for disk in App.disks:
                disk.update()

            # Draw base key
            pygame.draw.rect(App.screen, (0,0,0),[0,App.HEIGHT//2 - App.disks[-1].cone//2, 
                                                        App.WIDTH//2, App.disks[-1].cone])
            # Draw disks
            for disk in App.disks:
                disk.draw()
            
            # Draw Key (black line)
            for ind,line in enumerate(App.lines):
                pygame.draw.line(App.screen, (0,0,0), [0,App.HEIGHT//2 - App.disks[-1].cone//2 + ind], [line,App.HEIGHT//2 - App.disks[-1].cone//2 + ind])


            pygame.display.update()
            self.clock.tick(30)

        pygame.quit()

if __name__ == '__main__':
    App().run()