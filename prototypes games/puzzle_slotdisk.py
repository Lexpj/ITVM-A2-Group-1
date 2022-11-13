from math import radians, sin, cos
from random import randint, seed
import pygame
from pygame.locals import * 
seed(69)

class Slot:
    def __init__(self,radpos,disksize):
        self.color = (0,0,0)
        self.radpos = radpos
        self.disksize = disksize
        self.offset = 100
        self.arrowWidth = 15
        self.direction = 3

    def update(self,speed):
        self.radpos = (self.radpos+speed)%360

        self.offset += self.direction
        self.offset = max(min(self.offset,100),0)



    def draw(self):
        pygame.draw.lines(App.screen, self.color, True, [[App.WIDTH//2 + ((self.disksize*0.5)+self.offset)*cos(radians(self.radpos)), App.HEIGHT//2 + ((self.disksize*0.5)+self.offset)*sin(radians(self.radpos))],
                                                [App.WIDTH//2 + ((self.disksize)+self.offset)*cos(radians(self.radpos+self.arrowWidth)), App.HEIGHT//2 + ((self.disksize)+self.offset)*sin(radians(self.radpos+self.arrowWidth))],
                                                [App.WIDTH//2 + ((self.disksize)+self.offset)*cos(radians(self.radpos-self.arrowWidth)), App.HEIGHT//2 + ((self.disksize)+self.offset)*sin(radians(self.radpos-self.arrowWidth))]],5)

class Disk:

    def __init__(self,size,slots,speed=0,neg=True):
        self.size = size
        self.speedDisk = speed
        self.baseSpeed = 3
        self.speedCursor = self.baseSpeed
        self.direction = 1
        self.cursorPos = 0
        self.color = (255,255,255)
        self.cursorColor = (255,0,0)
        self.neg = neg
        self.nrSlots = slots

        
        self.slots = []
        self.mapSetup()

    def mapSetup(self):
        for i in range(0,360,360//self.nrSlots):
            self.slots.append(Slot(i,disksize=self.size))

    def detectSlot(self):
        for ind,slot in enumerate(self.slots):
            if slot.radpos-slot.arrowWidth <= self.cursorPos <= slot.radpos+slot.arrowWidth: # BUG bij de meest rechter arrow
                return ind
        return None
        
        


    def update(self):
        self.cursorPos = (self.cursorPos + self.direction*self.speedCursor)%360
        locked = 0

        for slot in self.slots:
            slot.update(self.speedDisk)
            if slot.offset == 0:
                locked += 1
        
        self.speedCursor = self.baseSpeed + ((locked+1)/self.nrSlots)*self.baseSpeed
        

    def draw(self):
        pygame.draw.circle(App.screen,self.color,[App.WIDTH//2,App.HEIGHT//2],self.size)
        pygame.draw.circle(App.screen,Color('gray'),[App.WIDTH//2,App.HEIGHT//2],self.size//2)

        for slot in self.slots:
            slot.draw()
        
        # cursor
        pygame.draw.line(App.screen, self.cursorColor, [App.WIDTH//2 + self.size*cos(radians(self.cursorPos)), App.HEIGHT//2 + self.size*sin(radians(self.cursorPos))],
                                                 [App.WIDTH//2 + self.size*1.3*cos(radians(self.cursorPos)), App.HEIGHT//2 + self.size*1.3*sin(radians(self.cursorPos))],5)


class App:

    def __init__(self):
        """Initialize pygame and the application."""
        pygame.init()
        flags = RESIZABLE
        App.WIDTH = 700
        App.HEIGHT = 700
        App.screen = pygame.display.set_mode((App.WIDTH, App.HEIGHT), flags)
        App.clock = pygame.time.Clock()

        App.disk = Disk(size=200,slots=8)
        
        App.running = True


    def run(self):
        """Run the main event loop."""
        while App.running:
            for event in pygame.event.get():
                if event.type == QUIT:
                    App.running = False
                elif event.type == pygame.KEYDOWN:
                    if event.key == pygame.K_SPACE:
                        if App.disk.neg:
                            App.disk.direction = -App.disk.direction
                        
                        foundslot = App.disk.detectSlot()
                        if foundslot != None:
                            App.disk.slots[foundslot].direction *= -1

            App.screen.fill(Color('gray'))

            # Update
            self.disk.update()

            # Draw disks
            self.disk.draw()
            
            # current slot
            curslot = App.disk.detectSlot()
            if curslot != None:
                App.disk.slots[curslot].color = (255,255,0)
                App.disk.slots[curslot].draw()
                App.disk.slots[curslot].color = (0,0,0)


            pygame.display.update()
            self.clock.tick(30)

        pygame.quit()

if __name__ == '__main__':
    App().run()