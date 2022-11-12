from math import radians
from random import randint, seed, choices
import pygame
from pygame.locals import * 


class Slot:

    def __init__(self,x,neg=False,speed=0.5):
        self.x = x
        self.width = 50
        self.height = 200
        self.neg = neg
        self.speed = 0.5

        # Block
        if not self.neg:
            self.cur = App.HEIGHT//2 + App.keyDepth//2 - self.height
            self.out = App.HEIGHT//2 - App.keyDepth//2 - self.height
        else:
            self.cur = App.HEIGHT//2 - App.keyDepth//2
            self.out = App.HEIGHT//2 + App.keyDepth//2
        
        self.bot = self.cur

    def update(self):
        if self.neg:
            self.cur = max(self.cur-self.speed, self.bot)
        else:
            self.cur = min(self.cur+self.speed, self.bot)
    
    def draw(self):
        # slot
        if self.neg:
            pygame.draw.rect(App.screen, (255,255,255), [self.x,App.HEIGHT//2 + App.keyDepth//2, self.width, self.height])
            pygame.draw.rect(App.screen, (0,0,0), [self.x+self.width//4,App.HEIGHT//2 + App.keyDepth//2, self.width//2, self.height])
        else:
            pygame.draw.rect(App.screen, (255,255,255), [self.x,App.HEIGHT//2 - App.keyDepth//2 - self.height, self.width, self.height])
            pygame.draw.rect(App.screen, (0,0,0), [self.x+self.width//4,App.HEIGHT//2 - App.keyDepth//2 - self.height, self.width//2, self.height])

        # block
        pygame.draw.rect(App.screen, (0,0,0), [self.x, self.cur, self.width, self.height])


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

        App.keyDepth = 100

        # For the puzzle itself
        App.slots = [
            Slot(100,neg=True,speed=0.2),
            Slot(200,speed=0.3),
            Slot(300,speed=0.2),
            Slot(400,neg=True,speed=0.3),
            Slot(500,speed=0.5),
        ]
        

    def getRange(self):
        length = 0
        while length < App.WIDTH and App.screen.get_at((length, App.HEIGHT//2)) == (255,255,255,255):
            length += 1
        pygame.draw.line(App.screen, (255,0,0), [0,App.HEIGHT//2], [length, App.HEIGHT//2],3)

    def getSlotIndex(self,x):
        for ind,slot in enumerate(App.slots):
            if slot.x <= x[0] <= slot.x+slot.width:
                if slot.cur <= x[1] <= slot.cur+slot.height:
                    return ind
        return None

    def run(self):
        """Run the main event loop."""
        dragging = False
        startco = None
        startslot = None
        while App.running:
            for event in pygame.event.get():
                if event.type == QUIT:
                    App.running = False
                elif event.type == pygame.MOUSEBUTTONDOWN and event.button == 1:
                    slot = self.getSlotIndex(event.pos)
                    if slot != None:
                        dragging = True
                        startco = event.pos
                        startslot = slot
                elif event.type == pygame.MOUSEMOTION:
                    if dragging:
                        m = pygame.mouse.get_pos()

                        if App.slots[startslot].neg:
                            App.slots[startslot].cur = min(max(App.slots[startslot].cur-(startco[1]-m[1]), App.slots[startslot].bot), App.slots[startslot].out)
                        else:
                            App.slots[startslot].cur = min(max(App.slots[startslot].cur-(startco[1]-m[1]), App.slots[startslot].out), App.slots[startslot].bot)

                        startco = m
                elif event.type == pygame.MOUSEBUTTONUP:
                    dragging = False


            # Update
            for slot in App.slots:
                slot.update()

            App.screen.fill(Color('gray'))

            # Key thing
            pygame.draw.rect(App.screen, (255,255,255), [0,App.HEIGHT//2 - App.keyDepth//2, App.WIDTH, App.keyDepth])

            # Draw tiles
            for slot in App.slots:
                slot.draw()
            
            self.getRange()

            pygame.display.update()
            self.clock.tick(30)

        pygame.quit()

if __name__ == '__main__':
    App().run()