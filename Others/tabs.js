import React, { useState } from 'react';
import { Tab, Tabs, TabList, TabPanel } from 'react-tabs';
import 'react-tabs/style/react-tabs.css';

function App() {
  const [tabIndex, setTabIndex] = useState(0);

  return (
    <Tabs selectedIndex={tabIndex} onSelect={index => setTabIndex(index)}>
      <TabList>
        <Tab>Imagen 1</Tab>
        <Tab>Imagen 2</Tab>
        <Tab>Imagen 3</Tab>
      </TabList>

      <TabPanel>
        <img src="LogIn.png" alt="Log In" />
      </TabPanel>
      <TabPanel>
        <img src="Register.png" alt="Register" />
      </TabPanel>
      <TabPanel>
        <img src="Home.png" alt="Home" />
      </TabPanel>
      <TabPanel>
        <img src="Newcomers.png" alt="Newcomers" />
      </TabPanel>
      <TabPanel>
        <img src="FindMyGame.png" alt="Find My Game" />
      </TabPanel>
      <TabPanel>
        <img src="MyLibrary.png" alt="My Library" />
      </TabPanel>
      <TabPanel>
        <img src="Settings.png" alt="Settings" />
      </TabPanel>
      <TabPanel>
        <img src="Help.png" alt="Help" />
      </TabPanel>
      <TabPanel>
        <img src="AdminPanel.png" alt="Admin Panel" />
      </TabPanel>
    </Tabs>
  );
}

export default App;