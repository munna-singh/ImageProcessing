import React, { Component } from 'react';
import {
  AppRegistry,
  StyleSheet,
  View,
  Text
} from 'react-native';

import SimilarFaces from './components/SimilarFaces';

const image_picker_options = {
  title: 'Select Photo',
  takePhotoButtonTitle: 'Take Photo...',
  chooseFromLibraryButtonTitle: 'Choose from Library...',
  cameraType: 'back',
  mediaType: 'photo',
  maxWidth: 480,
  quality: 1,
  noData: false,
};

//the API Key that you got from Microsoft Azure
const api_key = 'd6975a797c91410db46f9be6606f551e'; //face
const api_computer_vision = '84d895648cb9402aba03de99143b9085' //image

class RNSimilar extends Component {

  render() {
    return (
      <View style={styles.container}>
        <SimilarFaces imagePickerOptions={image_picker_options} apiKey={api_key} />
        <Text>How are you?</Text>
      </View>
    );
  }

}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#ccc',
  }
});

AppRegistry.registerComponent('face', () => RNSimilar);