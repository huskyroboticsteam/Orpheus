# Tennisball
## Code for tennis ball detection (Work in progress)

### Setup
#### Installing Object Detection API models
1. Enter project directory
2. **From the project directory**, run `scripts/install_models.sh`. This script will clone the Object Detection API from GitHub, copy it to the project directory, and then patch the model_main.py file with the modified version found in `scripts/`.
#### Installing libraries


### Running training job
1. Enter project directory
2. **From the project directory**, run `scripts/run_training.sh`. This script will call the Object Detection API's model_main.py.

### Compiling
1. Enter the project directory
2. Run `cmake .`
3. Run `make`
4. Executables should have been created.
