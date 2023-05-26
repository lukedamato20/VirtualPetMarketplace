App = {
  web3Provider: null,
  contracts: {},
  adoptedPets: [], // array to store adopted pets' ids
  selectedParents: [], // new array to store selected parents' pet ids
  petHistory: [], // array to store the children
  updatedPet: [], // array to store an updated pet
  currentPet: [],

  traits: {
    age: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20],
    body_colour: ['gray', 'red', 'orange', 'yellow', 'green', 'blue', 'indigo', 'pink'],
    maxfood_level: ['very_low', 'low', 'medium', 'high', 'very_high'],
    maxhappiness_level: ['very_low', 'low', 'medium', 'high', 'very_high'],
    maxenergy_level: ['very_low', 'low', 'medium', 'high', 'very_high'],
    hunger_level: ['very_low', 'low', 'medium', 'high', 'very_high'],
    sadness_level: ['very_low', 'low', 'medium', 'high', 'very_high'],
    laziness_level: ['very_low', 'low', 'medium', 'high', 'very_high'],
    fertility_counter: [1, 2, 3, 4, 5, 6, 7],
  },

  weights: {
    age: 0.15,
    maxfood_level: 0.125,
    maxhappiness_level: 0.125,
    maxenergy_level: 0.125,
    hunger_level: 0.15,
    sadness_level: 0.15,
    laziness_level: 0.15,
    fertility_counter: 0.2,
  },

  create_population: function () {
    class VirtualPet {
      constructor({
        id,
        name,
        age,
        body_colour,
        maxfood_level,
        maxhappiness_level,
        maxenergy_level,
        hunger_level,
        sadness_level,
        laziness_level,
        fertility_counter,
        picture,
      }) {
        this.id = id;
        this.name = name;
        this.age = age;
        this.body_colour = body_colour;
        this.maxfood_level = maxfood_level;
        this.maxhappiness_level = maxhappiness_level;
        this.maxenergy_level = maxenergy_level;
        this.hunger_level = hunger_level;
        this.sadness_level = sadness_level;
        this.laziness_level = laziness_level;
        this.fertility_counter = fertility_counter;
        this.picture = picture;
      }

      toString() {
        return `VirtualPet(id=${this.id}, name=${this.name}, size=${this.age}, body_colour=${this.body_colour}, maxfood_level=${this.maxfood_level}, maxhappiness_level=${this.maxhappiness_level}, maxenergy_level=${this.maxenergy_level}, hunger_level=${this.hunger_level}, sadness_level=${this.sadness_level}, laziness_level=${this.laziness_level}, fertility_counter=${this.fertility_counter})`;
      }
    }

    return new Promise((resolve, reject) => {
      let population = [];

      $.getJSON('../json/pets_metadata.json', function (data) {
        population = data.map((petData) => new VirtualPet(petData));
        resolve(population);
      });
    });
  },

  traits_fitness: function (value) {
    if (value === 'very_low') {
      return 1;
    } else if (value === 'low') {
      return 2;
    } else if (value === 'medium') {
      return 3;
    } else if (value === 'high') {
      return 4;
    } else if (value === 'very_high') {
      return 5;
    }
  },

  needs_fitness: function (value) {
    if (value === 'very_low') {
      return 5;
    } else if (value === 'low') {
      return 4;
    } else if (value === 'medium') {
      return 3;
    } else if (value === 'high') {
      return 2;
    } else if (value === 'very_high') {
      return 1;
    }
  },

  calculate_fitness: function (pet) {
    // Calculate the score for each trait's fitness function
    let age_score = pet.age;
    let maxfood_score = App.traits_fitness(pet.maxfood_level);
    let maxhappiness_score = App.traits_fitness(pet.maxhappiness_level);
    let maxenergy_score = App.traits_fitness(pet.maxenergy_level);
    let hunger_score = App.needs_fitness(pet.hunger_level);
    let sadness_score = App.needs_fitness(pet.sadness_level);
    let laziness_score = App.needs_fitness(pet.laziness_level);
    let fertility_score = pet.fertility_counter;

    // Calculate the weighted sum of the trait scores
    let final_score = (age_score * App.weights['age'] +
      maxfood_score * App.weights['maxfood_level'] +
      maxhappiness_score * App.weights['maxhappiness_level'] +
      maxenergy_score * App.weights['maxenergy_level'] +
      hunger_score * App.weights['hunger_level'] +
      sadness_score * App.weights['sadness_level'] +
      laziness_score * App.weights['laziness_level'] +
      fertility_score * App.weights['fertility_counter']);

    return final_score;
  },

  get_rating: function (score) {
    // fitness score of pet  98 :  1.0 WORST
    // fitness score of pet  99 :  5.6 BEST
    if (score <= 1) {
      return 'Very Poor';
    } else if (score < 2) {
      return 'Poor';
    } else if (score < 3) {
      return 'Average';
    } else if (score < 4) {
      return 'Good';
    } else if (score < 5) {
      return 'Great';
    } else if (score >= 5.6) {
      return 'Perfect';
    } else {
      return 'other';
    }
  },

  crossover: function (parent1, parent2, mutation_probability) {
    class VirtualPet {
      constructor({
        id,
        name,
        age,
        body_colour,
        maxfood_level,
        maxhappiness_level,
        maxenergy_level,
        hunger_level,
        sadness_level,
        laziness_level,
        fertility_counter,
        picture,
      }) {
        this.id = id;
        this.name = name;
        this.age = age;
        this.body_colour = body_colour;
        this.maxfood_level = maxfood_level;
        this.maxhappiness_level = maxhappiness_level;
        this.maxenergy_level = maxenergy_level;
        this.hunger_level = hunger_level;
        this.sadness_level = sadness_level;
        this.laziness_level = laziness_level;
        this.fertility_counter = fertility_counter;
        this.picture = picture;
      }

      toString() {
        return `VirtualPet(id=${this.id}, name=${this.name}, size=${this.age}, body_colour=${this.body_colour}, maxfood_level=${this.maxfood_level}, maxhappiness_level=${this.maxhappiness_level}, maxenergy_level=${this.maxenergy_level}, hunger_level=${this.hunger_level}, sadness_level=${this.sadness_level}, laziness_level=${this.laziness_level}, fertility_counter=${this.fertility_counter})`;
      }
    }

    const child_attributes = {};

    for (const attr_name in parent1) {
      if (attr_name == 'id') {
        child_attributes['id'] = (parent1['id'] + "_" + parent2['id'])
      }
      else if (attr_name == 'name') {
        child_attributes['name'] = (parent1['name'] + "_" + parent2['name'] + " child")
      }
      else if (attr_name == 'age') {
        child_attributes['age'] = 0;
      }
      else if (attr_name == 'picture') {
        child_attributes['picture'] = "images/" + child_attributes['body_colour'] + "_body.png"
      }
      else {
        // randomly choose which parent to get the attribute from
        if (Math.random() < 0.5) {
          child_attributes[attr_name] = parent1[attr_name];
        } else {
          child_attributes[attr_name] = parent2[attr_name];
        }
      }
    }

    // create a new VirtualPet instance with the child attributes
    const child = new VirtualPet(child_attributes);

    // randomly decide whether to apply mutation or not
    // if (Math.random() < mutation_probability) {
    //   mutate(child);
    // }

    return child;
  },

  mutate: function (child) {
    // Choose a random attribute to mutate
    const attribute = ['body_colour', 'maxfood_level', 'maxhappiness_level', 'maxenergy_level', 'hunger_level', 'sadness_level', 'laziness_level'][Math.floor(Math.random() * 8)];

    // Modify the attribute
    switch (attribute) {
      case 'body_colour':
        child.body_colour = ['gray', 'red', 'orange', 'yellow', 'green', 'blue', 'indigo', 'pink'][Math.floor(Math.random() * 8)];
        break;
      case 'maxfood_level':
        child.maxfood_level = ['very_low', 'low', 'medium', 'high', 'very_high'][Math.floor(Math.random() * 5)];
        break;
      case 'maxhappiness_level':
        child.maxhappiness_level = ['very_low', 'low', 'medium', 'high', 'very_high'][Math.floor(Math.random() * 5)];
        break;
      case 'maxenergy_level':
        child.maxenergy_level = ['very_low', 'low', 'medium', 'high', 'very_high'][Math.floor(Math.random() * 5)];
        break;
      case 'hunger_level':
        child.hunger_level = ['very_low', 'low', 'medium', 'high', 'very_high'][Math.floor(Math.random() * 5)];
        break;
      case 'sadness_level':
        child.sadness_level = ['very_low', 'low', 'medium', 'high', 'very_high'][Math.floor(Math.random() * 5)];
        break;
      case 'laziness_level':
        child.laziness_level = ['very_low', 'low', 'medium', 'high', 'very_high'][Math.floor(Math.random() * 5)];
        break;
    }

    return child;
  },

  translateAndShowData: function (attribute, value, template) {

    var petTemplate = $(template);

    if (attribute == '.maxfood_level' || attribute == '.maxhappiness_level' || attribute == '.maxenergy_level') {

      if (value == 200) {
        petTemplate.find(attribute).text('Very High').css({ "color": "#4dcc57", "font-weight": "bold" });
      } else if (value == 180) {
        petTemplate.find(attribute).text('High').css({ "color": "#acd91a", "font-weight": "bold" });
      } else if (value == 160) {
        petTemplate.find(attribute).text('Medium').css({ "color": "#ffcd1c", "font-weight": "bold" });
      } else if (value == 140) {
        petTemplate.find(attribute).text('Low').css({ "color": "#fd7e1f", "font-weight": "bold" });
      } else if (value == 120) {
        petTemplate.find(attribute).text('Very Low').css({ "color": "#e8344d", "font-weight": "bold" });
      }
    } else if (attribute == '.hunger_level' || attribute == '.sadness_level' || attribute == '.laziness_level') {
      if (value == 5) {
        petTemplate.find(attribute).text('Very High').css({ "color": "#e8344d", "font-weight": "bold" });
      } else if (value == 4) {
        petTemplate.find(attribute).text('High').css({ "color": "#fd7e1f", "font-weight": "bold" });
      } else if (value == 3) {
        petTemplate.find(attribute).text('Medium').css({ "color": "#ffcd1c", "font-weight": "bold" });
      } else if (value == 2) {
        petTemplate.find(attribute).text('Low').css({ "color": "#acd91a", "font-weight": "bold" });
      } else if (value == 1) {
        petTemplate.find(attribute).text('Very Low').css({ "color": "#4dcc57", "font-weight": "bold" });
      }
    } else if (attribute == 'body_colour') {
      if (value == 'gray') {
        petTemplate.find('img').attr('src', 'images/gray_body.png');
      } else if (value == 'red') {
        petTemplate.find('img').attr('src', 'images/red_body.png');
      } else if (value == 'orange') {
        petTemplate.find('img').attr('src', 'images/orange_body.png');
      } else if (value == 'yellow') {
        petTemplate.find('img').attr('src', 'images/yellow_body.png');
      } else if (value == 'green') {
        petTemplate.find('img').attr('src', 'images/green_body.png');
      } else if (value == 'blue') {
        petTemplate.find('img').attr('src', 'images/blue_body.png');
      } else if (value == 'purple') {
        petTemplate.find('img').attr('src', 'images/purple_body.png');
      } else if (value == 'pink') {
        petTemplate.find('img').attr('src', 'images/pink_body.png');
      }
    } else {
      petTemplate.find(attribute).text(value);
    }
  },

  changeColourAndText: function (attribute, value, template) {

    var petTemplate = $(template);

    if (attribute == '.maxfood_level' || attribute == '.maxhappiness_level' || attribute == '.maxenergy_level') {
      if (value == 'very_high') {
        petTemplate.find(attribute).text('Very High').css({ "color": "#4dcc57", "font-weight": "bold" });
      } else if (value == 'high') {
        petTemplate.find(attribute).text('High').css({ "color": "#acd91a", "font-weight": "bold" });
      } else if (value == 'medium') {
        petTemplate.find(attribute).text('Medium').css({ "color": "#ffcd1c", "font-weight": "bold" });
      } else if (value == 'low') {
        petTemplate.find(attribute).text('Low').css({ "color": "#fd7e1f", "font-weight": "bold" });
      } else if (value == 'very_low') {
        petTemplate.find(attribute).text('Very Low').css({ "color": "#e8344d", "font-weight": "bold" });
      }
    } else if (attribute == '.hunger_level' || attribute == '.sadness_level' || attribute == '.laziness_level') {
      if (value == 'very_high') {
        petTemplate.find(attribute).text('Very High').css({ "color": "#e8344d", "font-weight": "bold" });
      } else if (value == 'high') {
        petTemplate.find(attribute).text('High').css({ "color": "#fd7e1f", "font-weight": "bold" });
      } else if (value == 'medium') {
        petTemplate.find(attribute).text('Medium').css({ "color": "#ffcd1c", "font-weight": "bold" });
      } else if (value == 'low') {
        petTemplate.find(attribute).text('Low').css({ "color": "#acd91a", "font-weight": "bold" });
      } else if (value == 'very_low') {
        petTemplate.find(attribute).text('Very Low').css({ "color": "#4dcc57", "font-weight": "bold" });
      }
    } else if (attribute == '.age') {
      if (value == '0' || value == '1' || value == '2' || value == '3') {
        petTemplate.find(attribute).text(value).css({ "color": "#4dcc57", "font-weight": "bold" });
      } else if (value == '4' || value == '5' || value == '6' || value == '7') {
        petTemplate.find(attribute).text(value).css({ "color": "#ffcd1c", "font-weight": "bold" });
      } else if (value == '8' || value == '9' || value == '10') {
        petTemplate.find(attribute).text(value).css({ "color": "#e8344d", "font-weight": "bold" });
      }
    } else if (attribute == '.fertility_counter') {
      if (value == '1' || value == '2') {
        petTemplate.find(attribute).text(value).css({ "color": "#e8344d", "font-weight": "bold" });
      } else if (value == '3' || value == '4' || value == '5') {
        petTemplate.find(attribute).text(value).css({ "color": "#ffcd1c", "font-weight": "bold" });
      } else if (value == '6' || value == '7') {
        petTemplate.find(attribute).text(value).css({ "color": "#4dcc57", "font-weight": "bold" });
      }
    } else {
      petTemplate.find(attribute).text(value);
    }

  },

  // _______________________________________________________________________

  init: async function () {

    // Load pets.
    $.getJSON('../json/pets_metadata.json', function (data) {
      var petsRow = $('#petsRow');
      var petTemplate = $('#petTemplate');

      // {'very_low', 'low', 'medium', 'high', 'very_high'},
      for (i = 0; i < data.length; i++) {
        petTemplate.find('.panel-title').text(data[i].name);
        petTemplate.find('img').attr('src', data[i].picture);

        App.changeColourAndText('.id', data[i].id, '#petTemplate')
        App.changeColourAndText('.age', data[i].age, '#petTemplate')
        App.changeColourAndText('.maxfood_level', data[i].maxfood_level, '#petTemplate')
        App.changeColourAndText('.maxhappiness_level', data[i].maxhappiness_level, '#petTemplate')
        App.changeColourAndText('.maxenergy_level', data[i].maxenergy_level, '#petTemplate')
        App.changeColourAndText('.hunger_level', data[i].hunger_level, '#petTemplate')
        App.changeColourAndText('.sadness_level', data[i].sadness_level, '#petTemplate')
        App.changeColourAndText('.laziness_level', data[i].laziness_level, '#petTemplate')
        App.changeColourAndText('.fertility_counter', data[i].fertility_counter, '#petTemplate')

        petTemplate.find('.btn-adopt').attr('data-id', data[i].id);
        petsRow.append(petTemplate.html());
      }
    });

    return await App.initWeb3();
  },

  initWeb3: async function () {
    // Modern dapp browsers...
    if (window.ethereum) {
      App.web3Provider = window.ethereum;
      try {
        // Request account access
        await window.ethereum.enable();
      } catch (error) {
        // User denied account access...
        console.error("User denied account access")
      }
    }
    // Legacy dapp browsers...
    else if (window.web3) {
      App.web3Provider = window.web3.currentProvider;
    }
    // If no injected web3 instance is detected, fall back to Ganache
    else {
      App.web3Provider = new Web3.providers.HttpProvider('http://localhost:3000');
    }
    web3 = new Web3(App.web3Provider);

    return App.initContract();
  },

  initContract: function () {
    $.getJSON('Adoption.json', function (data) {
      // Get the necessary contract artifact file and instantiate it with @truffle/contract
      var AdoptionArtifact = data;
      App.contracts.Adoption = TruffleContract(AdoptionArtifact);

      // Set the provider for our contract
      App.contracts.Adoption.setProvider(App.web3Provider);

      // Use our contract to retrieve and mark the adopted pets
      return App.markAdopted();
    });

    return App.bindEvents();
  },

  bindEvents: function () {
    $(document).on('click', '.btn-adopt', App.handleAdopt);
  },

  markAdopted: function () {
    var adoptionInstance;

    App.contracts.Adoption.deployed().then(function (instance) {
      adoptionInstance = instance;

      return adoptionInstance.getAdopters.call();
    }).then(function (adopters) {
      for (i = 0; i < adopters.length; i++) {
        if (adopters[i] !== '0x0000000000000000000000000000000000000000') {
          $('.panel-pet').eq(i).find('button').text('Adopted!').attr('disabled', true);
        }
      }
    }).catch(function (err) {
      console.log(err.message);
    });
  },

  clearAdopted: function () {

    console.log("clearing buttons")

    App.adoptedPets = [];
    App.selectedParents = [];
    App.petHistory = [];
    App.updatedPet = [];
    App.FixUpdatedPet = [];

    // localStorage.removeItem('adoptedPets');
    // localStorage.removeItem('selectedParents');
    // localStorage.removeItem('petHistory');
    // localStorage.removeItem('updatedPet');
    localStorage.clear();

    $.getJSON('../json/pets_metadata.json', function (data) {
      for (i = 0; i < data.length; i++) {
        $('.panel-pet').eq(i).find('button').text('Success').attr('disabled', false);
      }
    });
  },

  handleAdopt: function (event) {
    event.preventDefault();

    var petId = parseInt($(event.target).data('id'));

    var adoptionInstance;

    web3.eth.getAccounts(function (error, accounts) {
      if (error) {
        console.log(error);
      }

      var account = accounts[0];

      App.contracts.Adoption.deployed().then(function (instance) {
        adoptionInstance = instance;

        // Execute adopt as a transaction by sending account
        return adoptionInstance.adopt(petId, { from: account });
      }).then(function (result) {

        // Retrieve previously stored adopted pets list from local storage
        var storedPets = localStorage.getItem('adoptedPets');

        if (storedPets) {
          App.adoptedPets = JSON.parse(storedPets);
        }

        // Append new pet ID to the list of adopted pets
        App.adoptedPets.push(petId);

        // Store the updated adopted pets list back in local storage
        localStorage.setItem('adoptedPets', JSON.stringify(App.adoptedPets));

        return App.markAdopted();
      }).catch(function (err) {
        console.log(err.message);
      });
    });
  },

  loadCollection: function () {
    // TODO check if local storage is empty and alert user
    $.getJSON('../json/pets_metadata.json', function (data) {

      $('#collection').addClass('active');

      App.adoptedPets = []

      var petsCollectionRow = $('#petsCollectionRow');
      var petCollectionTemplate = $('#petCollectionTemplate');
      App.adoptedPets = App.adoptedPets.concat(JSON.parse(localStorage.getItem('adoptedPets')) || []);

      console.log("Full list: ", App.adoptedPets)

      if (App.selectedParents.length < 2) {
        $('#btn-breed').prop('disabled', true);
      } else {
        $('#btn-breed').prop('disabled', false);
      }

      var myPetsData = [];

      for (x = 0; x < App.adoptedPets.length; x++) {
        for (i = 0; i < data.length; i++) {
          // check if the pet is adopted
          if (App.adoptedPets[x] == data[i].id) {
            const fitness_pet = App.calculate_fitness(data[i]);
            const rating_pet = App.get_rating(fitness_pet);

            petCollectionTemplate.find('.panel-title').text(data[i].name);
            petCollectionTemplate.find('img').attr('src', data[i].picture);

            App.changeColourAndText('.id', data[i].id, '#petCollectionTemplate')
            App.changeColourAndText('.age', data[i].age, '#petCollectionTemplate')
            App.changeColourAndText('.maxfood_level', data[i].maxfood_level, '#petCollectionTemplate')
            App.changeColourAndText('.maxhappiness_level', data[i].maxhappiness_level, '#petCollectionTemplate')
            App.changeColourAndText('.maxenergy_level', data[i].maxenergy_level, '#petCollectionTemplate')
            App.changeColourAndText('.hunger_level', data[i].hunger_level, '#petCollectionTemplate')
            App.changeColourAndText('.sadness_level', data[i].sadness_level, '#petCollectionTemplate')
            App.changeColourAndText('.laziness_level', data[i].laziness_level, '#petCollectionTemplate')
            App.changeColourAndText('.fertility_counter', data[i].fertility_counter, '#petCollectionTemplate')

            petCollectionTemplate.find('.fitness_ranking').text(rating_pet);

            petCollectionTemplate.find('.btn-selectParent').attr('data-id', data[i].id);
            petCollectionTemplate.find('.btn-playWithPet').attr('data-id', data[i].id);

            petsCollectionRow.append(petCollectionTemplate.html());

            // Add the pet to the local storage array
            myPetsData.push(data[i]);
          }
        }
      }

      // Store the updated myPetsData array back in local storage
      localStorage.setItem('myPetsData', JSON.stringify(myPetsData));

      console.log("Pets Data: ", JSON.parse(localStorage.getItem('myPetsData')));
    });
  },

  handleParentSelection: function (event) {
    event.preventDefault();

    var petId = parseInt($(event.target).data('id'));

    console.log("Handling Parent Selection...");
    console.log(App.selectedParents.length)
    if (App.selectedParents.length < 2) {
      // Check if the pet is already selected
      if (App.selectedParents.includes(petId)) {
        alert("You have already selected this pet!");
        return;
      }

      $(`button[data-id="${petId}"]`).prop('disabled', true).text('Selected!');

      $('#btn-breed').prop('disabled', false);

      // Add the pet to the selected parents array
      App.selectedParents.push(petId);
      if (App.selectedParents.length == 2) {
        $('#collection').removeClass('active');
        $('#breeding').addClass('active');
        $('#result').removeClass('active');
      }

    }
    else {
      alert("You can only select a maximum of 2 pets!");
    }

    // Update the selected pets in the template
    var petsBreedingRow = $('#petsBreedingRow');
    var petBreedingTemplate = $('#petBreedingTemplate');
    petsBreedingRow.empty();

    $.getJSON('../json/pets_metadata.json', function (data) {
      for (var i = 0; i < App.selectedParents.length; i++) {
        for (var j = 0; j < data.length; j++) {
          if (App.selectedParents[i] == data[j].id) {
            const fitness_pet = App.calculate_fitness(data[j]);
            const rating_pet = App.get_rating(fitness_pet);

            var clonedTemplate = petBreedingTemplate.clone();

            clonedTemplate.find('.panel-title').text(data[j].name);
            clonedTemplate.find('img').attr('src', data[j].picture);

            App.changeColourAndText('.id', data[j].id, clonedTemplate)
            App.changeColourAndText('.age', data[j].age, clonedTemplate)
            App.changeColourAndText('.maxfood_level', data[j].maxfood_level, clonedTemplate)
            App.changeColourAndText('.maxhappiness_level', data[j].maxhappiness_level, clonedTemplate)
            App.changeColourAndText('.maxenergy_level', data[j].maxenergy_level, clonedTemplate)
            App.changeColourAndText('.hunger_level', data[j].hunger_level, clonedTemplate)
            App.changeColourAndText('.sadness_level', data[j].sadness_level, clonedTemplate)
            App.changeColourAndText('.laziness_level', data[j].laziness_level, clonedTemplate)
            App.changeColourAndText('.fertility_counter', data[j].fertility_counter, clonedTemplate)

            clonedTemplate.find('.fitness_ranking').text(rating_pet);

            petsBreedingRow.append(clonedTemplate.html());
          }
        }
      }
    });
  },

  handleBreeding: function (event) {
    event.preventDefault();

    var petId = parseInt($(event.target).data('id'));

    function show_child(child) {

      console.log(child)
      const fitness_child = App.calculate_fitness(child);
      const rating_child = App.get_rating(fitness_child);

      var petsChildRow = $('#petsChildRow');
      var petChildTemplate = $('#petChildTemplate');

      petChildTemplate.find('.panel-title').text(child.name);
      petChildTemplate.find('img').attr('src', child.picture);

      App.changeColourAndText('.id', child.id, '#petChildTemplate')
      App.changeColourAndText('.age', child.age, '#petChildTemplate')
      App.changeColourAndText('.maxfood_level', child.maxfood_level, '#petChildTemplate')
      App.changeColourAndText('.maxhappiness_level', child.maxhappiness_level, '#petChildTemplate')
      App.changeColourAndText('.maxenergy_level', child.maxenergy_level, '#petChildTemplate')
      App.changeColourAndText('.hunger_level', child.hunger_level, '#petChildTemplate')
      App.changeColourAndText('.sadness_level', child.sadness_level, '#petChildTemplate')
      App.changeColourAndText('.laziness_level', child.laziness_level, '#petChildTemplate')
      App.changeColourAndText('.fertility_counter', child.fertility_counter, '#petChildTemplate')

      petChildTemplate.find('.parent1_id').text(App.selectedParents[0]);
      petChildTemplate.find('.parent2_id').text(App.selectedParents[1]);
      petChildTemplate.find('.fitness_ranking').text(rating_child);

      petChildTemplate.find('.btn-playWithPet').attr('data-id', child.id);

      petsChildRow.append(petChildTemplate.html());
      $('#collection').removeClass('active');
      $('#breeding').removeClass('active');
      $('#result').addClass('active');
    }

    function updateBreedingCounter(parent1, parent2) {
      var myPetsData = JSON.parse(localStorage.getItem('myPetsData'));

      // Search for pets with parent1 and parent2 IDs
      for (var i = 0; i < myPetsData.length; i++) {
        var petData = myPetsData[i];
        if (petData.id == parent1 || petData.id == parent2) {
          // Decrement the fertility_counter by 1
          petData.fertility_counter -= 1;
          console.log("reduced pets counter")
        }
      }

      // Store the updated myPetsData array back to the localStorage
      localStorage.setItem('myPetsData', JSON.stringify(myPetsData));
    }

    async function main() {
      console.log("starting function")
      // Creating the Initial Population
      const population = await App.create_population();

      console.log("created population")
      console.log(population)

      // GET PARENT ID FROM JS
      const parent1_id = App.selectedParents[0];
      const parent2_id = App.selectedParents[1];
      let parent1 = null;
      let parent2 = null;

      console.log("created parents")

      // selecting the parents
      for (let i = 0; i < population.length; i++) {
        let pet = population[i];
        if (parseInt(pet.id) === parseInt(parent1_id)) {
          parent1 = pet;
        } else if (parseInt(pet.id) === parseInt(parent2_id)) {
          parent2 = pet;
        }
      }

      console.log("got parents' data")

      // 0.1: 10% chance of mutation
      const mutation_probability = 0;

      const fitness_parent1 = App.calculate_fitness(parent1);
      const rating_parent1 = App.get_rating(fitness_parent1);
      const fitness_parent2 = App.calculate_fitness(parent2);
      const rating_parent2 = App.get_rating(fitness_parent2);

      console.log("got fitness data")

      // crossover using parent1 and parent2
      if (parent1 === null || parent2 === null) {
        console.log("Could not find one or both parents in population");
      } else {
        const child = App.crossover(parent1, parent2, mutation_probability);
        console.log("done!")
        localStorage.setItem('child', JSON.stringify(child));
        updateBreedingCounter(App.selectedParents[0], App.selectedParents[1]);
        show_child(child);

      }
    }

    main()
  },

  handlePlayButton: function (event) {

    event.preventDefault();

    var petIdPlay = $(event.target).data('id');
    console.log("Play button pressed: " + petIdPlay);

    var myPetsData = JSON.parse(localStorage.getItem('myPetsData'));
    var adoptedPets = JSON.parse(localStorage.getItem('adoptedPets'));

    for (var i = 0; i < myPetsData.length; i++) {
      if (petIdPlay == myPetsData[i].id) {
        var currentPet = myPetsData.splice(i, 1)[0]; // remove pet from myPetsData array and get the removed pet object
        var remadoptedPets = adoptedPets.splice(i, 1)[0];

        localStorage.setItem('myPetsData', JSON.stringify(myPetsData)); // update myPetsData in local storage
        localStorage.setItem('adoptedPets', JSON.stringify(adoptedPets)); // update myPetsData in local storage
        localStorage.setItem('currentPet', JSON.stringify(currentPet)); // add the removed pet to currentPet in local storage

        console.log("Selected Pet is not a child");
        console.log("myPetsData: ", JSON.parse(localStorage.getItem('myPetsData')));
        console.log("myPetsData: ", localStorage.getItem('adoptedPets'));
        console.log("currentPet: ", JSON.parse(localStorage.getItem('currentPet')));

        window.location.href = '/play';
      }
    }
  },

  handlePlayChildButton: function () {
    App.child = JSON.parse(localStorage.getItem('child'));

    console.log("Selected Pet is a child")

    localStorage.setItem('currentPet', JSON.stringify(App.child));
    console.log("Child data is: ", JSON.parse(localStorage.getItem('currentPet')))

    window.location.href = '/play';
  },

  // handleHistoryCollection: function () {
  //   $.getJSON('../json/pets_metadata.json', function (data) {

  //     var petHistoryRow = $('#petHistoryRow');
  //     var petHistoryTemplate = $('#petHistoryTemplate');

  //     if (localStorage.getItem('petHistory') !== null) {
  //       App.petHistory = App.petHistory.concat(JSON.parse(localStorage.getItem('petHistory')) || []);
  //     }

  //     console.log("Full list: ", App.petHistory)

  //     for (i = 0; i < data.length; i++) {
  //       for (x = 0; x < App.petHistory.length; x++) {
  //         // check if the pet is adopted and does not already exist in App.petHistory
  //         if (App.petHistory[x].id == data[i].id && !App.petHistory.includes(data[i])) {
  //           console.log("pet found");
  //           //petHistoryTemplate.find('.panel-title').text(data[i].name);
  //           petHistoryTemplate.find('img').attr('src', data[i].picture);
  //           petHistoryTemplate.find('age').attr('src', data[i].age);
  //           petHistoryTemplate.find('.id').text(data[i].id);
  //           // petHistoryTemplate.find('.fertility_counter').text(data[i].fertility_counter);
            
  //           petHistoryTemplate.find('.maxfood_level').text(data[i].maxfood_level);
  //           petHistoryTemplate.find('.maxhappiness_level').text(data[i].maxhappiness_level);
  //           petHistoryTemplate.find('.maxenergy_level').text(data[i].maxenergy_level);
  //           petHistoryTemplate.find('.hunger_level').text(data[i].hunger_level);
  //           petHistoryTemplate.find('.sadness_level').text(data[i].sadness_level);
  //           petHistoryTemplate.find('.laziness_level').text(data[i].laziness_level);

  //           petHistoryTemplate.find('.btn-loadPet').attr('data-id', data[i].id);

  //           petHistoryRow.append(petHistoryTemplate.html());
  //           // add the pet to App.petHistory
  //           App.petHistory.push(data[i]);
  //         }
  //         else {
  //           console.log("no pet was found or pet already exists in App.petHistory")
  //         }
  //       }
  //     }
  //   });
  // },

  handleLoadingPet: function (event) {

    event.preventDefault();

    var petIDLoad = $(event.target).data('id');

    // get petHistory from localStorage
    var petHistory = JSON.parse(localStorage.getItem('petHistory'));
    console.log(petHistory)

    for (var i = 0; i < petHistory.length; i++) {
      if (petHistory[i].id == petIDLoad) {
        // Found matching pet, do something
        console.log('Found pet with ID ' + petIDLoad);

        localStorage.setItem('currentPet', JSON.stringify(petHistory[i]));
        console.log(petHistory[i])
        break;
      }
      else {
        console.log('pet not found with ID ' + petIDLoad);
      }
    }

    window.location.href = '/play';
  },

  handleNewPetData: function () {
    // 0: currentId
    // 1: BodyColour
    // 2: age
    // 3: current_food
    // 4: current_happiness
    // 5: current_energy
    // 6: MaxFood
    // 7: MaxHappiness
    // 8: MaxEnergy
    // 9: BreedingCounter
    // 10: FoodTickRate
    // 11: HappinessTickRate
    // 12: EnergyTickRate
    // 13: isDead

    App.updatedPet = JSON.parse(localStorage.getItem('updatedPet'))

    if (App.updatedPet != null) {
      const FixUpdatedPet = {
        id: App.updatedPet[0],
        age: App.updatedPet[2],
        current_food: App.updatedPet[3],
        current_happiness: App.updatedPet[4],
        current_energy: App.updatedPet[5],
        maxfood_level: App.updatedPet[6],
        maxhappiness_level: App.updatedPet[7],
        maxenergy_level: App.updatedPet[8],
        hunger_level: App.updatedPet[10],
        fertility_counter: App.updatedPet[9],
        sadness_level: App.updatedPet[11],
        laziness_level: App.updatedPet[12],
        body_colour: App.updatedPet[1],
        isDead: App.updatedPet[13]
      };

      // check if pethistory exists in localStorage
      let petHistory = JSON.parse(localStorage.getItem('petHistory')) || [];

      // check if the updated pet already exists in petHistory
      const index = petHistory.findIndex(pet => pet.id === App.updatedPet[0]);

      // if the pet already exists, replace it with the updated pet
      if (index !== -1) {
        petHistory[index] = FixUpdatedPet;
      } else {
        // otherwise, add the updated pet to petHistory
        petHistory.push(FixUpdatedPet);
      }

      // update pethistory in localStorage
      localStorage.setItem('petHistory', JSON.stringify(petHistory));

      console.log("New Added Pet: ", FixUpdatedPet);
      console.log("Pet History: ", petHistory);

      var petsRow = $('#petHistoryRow');
      var petTemplate = $('#petHistoryTemplate');

      // loop through petHistory array and show data for each pet
      petHistory.forEach(function (pet) {
        // var petTemplate = $('#petHistoryTemplate').clone(); // clone the template

        App.changeColourAndText('.id', pet.id, petTemplate)
        App.changeColourAndText('.age', pet.age, petTemplate)
        App.changeColourAndText('.maxfood_level', pet.maxfood_level, petTemplate)
        App.changeColourAndText('.maxhappiness_level', pet.maxhappiness_level, petTemplate)
        App.changeColourAndText('.maxenergy_level', pet.maxenergy_level, petTemplate)
        App.changeColourAndText('.hunger_level', pet.hunger_level, petTemplate)
        App.changeColourAndText('.sadness_level', pet.sadness_level, petTemplate)
        App.changeColourAndText('.laziness_level', pet.laziness_level, petTemplate)
        App.changeColourAndText('.fertility_counter', pet.fertility_counter, petTemplate)

        App.translateAndShowData('body_colour', pet.body_colour, petTemplate)
        console.log(pet.body_colour)

        if (pet.isDead == 'False') {
          petTemplate.find('.btn-loadPet').attr('data-id', pet.id);

        } else if ((pet.isDead == 'True')) {
          petTemplate.find('.btn-loadPet').attr('data-id', pet.id).text('Dead!').attr('disabled', true);
        }

        petsRow.append(petTemplate.html());
      });

      // food: App.updatedPet[3],
      // happiness: App.updatedPet[4],
      // energy: App.updatedPet[5]
    }
  }

};

$(function () {
  $(window).load(function () {
    App.init();
  });

  $(document).ready(function () {
    if (window.location.pathname == '/collection.html') {
      App.loadCollection();
      $(document).on('click', '.btn-selectParent', App.handleParentSelection);
      $(document).on('click', '.btn-breed', App.handleBreeding);
      $(document).on('click', '.btn-playWithPet', App.handlePlayButton);
      $(document).on('click', '.btn-playWithChild', App.handlePlayChildButton);

    }
    if (window.location.pathname == '/logout') {

      // TODO Clear buttons
      App.clearAdopted();
      window.location.href = '/';
    }
    if (window.location.pathname == '/play') {
      window.location.href = '/dev/index.html';

    }
    if (window.location.pathname == '/account.html') {
      App.handleNewPetData();
      $(document).on('click', '.btn-loadPet', App.handleLoadingPet);
    }
  });
});