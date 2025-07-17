enum Icons {
	eye = '/identity/assets/images/sprite.svg#eye',
	eyeClosed = '/identity/assets/images/sprite.svg#eye-closed',
	checked = '/identity/assets/images/sprite.svg#checked',
}

(function () {
	'use strict';

	const PASSWORD_REGEX: RegExp = new RegExp(/^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()"№;:?_+])(?!.*[а-яА-Я ]).{8,30}$/);
	const EMAIL_REGEX: RegExp = new RegExp('^[\\w]+([\\._-][\\w]+)*@[a-zа-я0-9]+([\\.-][a-zа-я0-9]+)*(\\.[a-zа-я0-9]{2,})$');
	const USERNAME_REGEX: RegExp = new RegExp('^[a-zA-Z0-9]{4,30}$');

	const $login: HTMLInputElement = document.getElementById('login') as HTMLInputElement;
	const $username: HTMLInputElement = document.getElementById('username') as HTMLInputElement;
	const $password: HTMLInputElement = document.getElementById('password') as HTMLInputElement;
	const $newPassword: HTMLInputElement = document.getElementById('newPassword') as HTMLInputElement;
	const $confirmedNewPassword: HTMLInputElement = document.getElementById('confirmedNewPassword') as HTMLInputElement;
	const $passwordVisibilityButton = document.querySelectorAll('.password-visibility');
	const $email: HTMLInputElement = document.getElementById('email') as HTMLInputElement;
	const $userAgreementCheckbox: HTMLInputElement = document.getElementById('userAgreement') as HTMLInputElement;

	const $submitButton: HTMLButtonElement = document.getElementById('submitButton') as HTMLButtonElement;
	const $formElement: HTMLFormElement = document.getElementById('form') as HTMLFormElement;

	const $serverFeedbacks: NodeListOf<Element> = document.querySelectorAll('.server-feedback');
	const $serverErrorFields: NodeListOf<Element> = document.querySelectorAll('.invalid');

	const $digitCodeInput: HTMLInputElement = document.querySelector<HTMLInputElement>('#digit-code-input');
	const $digitCodeButton: HTMLButtonElement = document.querySelector<HTMLButtonElement>('#digit-code-button');

	const $forgotPasswordTabs: HTMLDivElement = document.querySelector<HTMLDivElement>('#forgot-password-tabs');

	if ($userAgreementCheckbox !== null) {
		$userAgreementCheckbox.addEventListener('input', onUserAgreementInput);
	}

	if ($email !== null) {
		$email.addEventListener('input', onEmailInput);
		$email.addEventListener('blur', onInputBlur);
	}

	if ($username !== null) {
		$username.addEventListener('input', onUsernameInput);
		$username.addEventListener('blur', onInputBlur);
	}

	if ($login !== null) {
		$login.addEventListener('input', checkLogin);
	}

	if ($password !== null) {
		$password.addEventListener('input', checkPasswordValidation);
	}

	if ($newPassword !== null) {
		$newPassword.addEventListener('input', onPasswordNewPasswordInput);
		$newPassword.addEventListener('blur', onInputBlur);
	}

	if ($confirmedNewPassword !== null) {
		$confirmedNewPassword.addEventListener('input', onPasswordConfirmPasswordInput);
		$confirmedNewPassword.addEventListener('blur', onInputBlur);
	}

	if ($passwordVisibilityButton !== null) {
		$passwordVisibilityButton
			.forEach(visibilityButton => visibilityButton.addEventListener('click', togglePasswordVisibility));
	}

	if ($digitCodeInput !== null && $digitCodeButton !== null) {
		createDigitCodeInput();
	}

	if ($forgotPasswordTabs !== null) {
		checkForgotPasswordValidation();
	}

	function onUserAgreementInput(): void {
		if ($userAgreementCheckbox) {
			$userAgreementCheckbox.value = $userAgreementCheckbox.checked.toString();
		}

		checkFormValidity();
	}

	function checkLogin(): void {
		checkInputEmpty($login);
		checkFormValidity();
		resetServerValidation();
	}

	function checkPasswordValidation(): void {
		checkInputEmpty($password);
		checkFormValidity();
		resetServerValidation();
	}

	function onInputBlur(this: Element, event: Event): void {
		const $inputElement: HTMLInputElement = this as HTMLInputElement;
		setTouched($inputElement);
	}

	function onUsernameInput(this: Element, event: Event): void {
		const $inputElement: HTMLInputElement = this as HTMLInputElement;
		setTouched($inputElement);

		checkLoginValidation();

		checkInputEmpty($inputElement);
		checkFormValidity();
		resetServerValidation();
	}

	function onEmailInput(this: Element, event: Event): void {
		const $inputElement: HTMLInputElement = this as HTMLInputElement;
		setTouched($inputElement);

		checkEmailValidation();

		checkInputEmpty($inputElement);
		checkFormValidity();
		resetServerValidation();
	}

	function onPasswordNewPasswordInput(this: Element, event: Event): void {
		const $inputElement: HTMLInputElement = this as HTMLInputElement;
		setTouched($inputElement);

		checkNewPasswordValidation();

		checkInputEmpty($inputElement);
		checkFormValidity();
		resetServerValidation();
	}

	function onPasswordConfirmPasswordInput(this: Element, event: Event): void {
		const $inputElement: HTMLInputElement = this as HTMLInputElement;
		setTouched($inputElement);

		checkConfirmPasswordValidation();

		checkInputEmpty($inputElement);
		checkFormValidity();
		resetServerValidation();
	}

	function setTouched($inputElement: HTMLInputElement): void {
		$inputElement.classList.add('touched');
		$inputElement.parentElement.querySelector('.validation-error-message').classList.add('active');
	}

	function checkLoginValidation(): void {
		const $usernameMsgElement: HTMLDivElement = $username.parentElement.querySelector('.validation-error-message') as HTMLDivElement;
		const confirmUsernameErrorMessage: string = isUsernameMatchPattern($username.value);

		if ($usernameMsgElement) {
			$usernameMsgElement.innerText = confirmUsernameErrorMessage || '';
		}

		if (confirmUsernameErrorMessage) {
			$username.classList.add('invalid');
		} else {
			$username.classList.remove('invalid');
		}

		$username.setCustomValidity(confirmUsernameErrorMessage || '');
	}

	function checkEmailValidation(): void {
		const $emailMsgElement: HTMLDivElement = $email.parentElement.querySelector('.validation-error-message') as HTMLDivElement;
		const confirmEmailErrorMessage: string = isEmailMatchPattern($email.value);

		if ($emailMsgElement) {
			$emailMsgElement.innerText = confirmEmailErrorMessage || '';
		}

		if (confirmEmailErrorMessage) {
			$email.classList.add('invalid');
		} else {
			$email.classList.remove('invalid');
		}

		$email.setCustomValidity(confirmEmailErrorMessage || '');
	}

	function checkNewPasswordValidation(): void {
		const $newPassMsgElement: HTMLDivElement = $newPassword.parentElement.querySelector('.validation-error-message');
		const samePasswordsErrorMessage: string = comparePasswords();
		const newPassErrorMessage = isPasswordMatchPattern($newPassword.value);

		if (newPassErrorMessage) {
			$newPassword.classList.add('invalid');
		} else {
			$newPassword.classList.remove('invalid');
		}

		if ($newPassMsgElement) {
			$newPassMsgElement.innerText = newPassErrorMessage || '';
		}
		$newPassword.setCustomValidity(newPassErrorMessage || '');
	}

	function checkConfirmPasswordValidation(): void {
		const $confirmPassMsgElement: HTMLDivElement = $confirmedNewPassword.parentElement.querySelector('.validation-error-message') as HTMLDivElement;
		const samePasswordsErrorMessage: string = comparePasswords();

		if (samePasswordsErrorMessage) {
			$confirmedNewPassword.classList.add('invalid');
		} else {
			$confirmedNewPassword.classList.remove('invalid');
		}

		if ($confirmPassMsgElement) {
			$confirmPassMsgElement.innerText = samePasswordsErrorMessage || '';
		}
		$confirmedNewPassword.setCustomValidity(samePasswordsErrorMessage || '');
	}

	function togglePasswordVisibility(this: Element, event: Event): void {
		const icon: Element = this.querySelector('svg use');
		const passElement: HTMLElement = document.getElementById((this as HTMLButtonElement).dataset.field);

		if (passElement.getAttribute('type') === 'password') {
			passElement.setAttribute('type', 'text');
			icon.setAttribute('href', Icons.eye);
		} else {
			passElement.setAttribute('type', 'password');
			icon.setAttribute('href', Icons.eyeClosed);
		}
	}

	function checkFormValidity(): void {
		if ($submitButton && $formElement) {
			$submitButton.disabled = !$formElement.checkValidity() || ($userAgreementCheckbox && !$userAgreementCheckbox.checked);
		}
	}

	function checkInputEmpty(input: HTMLInputElement): void {
		if (input.value == null || input.value === '') {
			input.classList.add('empty');
		} else {
			input.classList.remove('empty');
		}
	}

	function resetServerValidation(): void {
		if ($serverFeedbacks) {
			$serverFeedbacks.forEach(feedback => feedback.classList.add('d-none'));
		}

		if ($serverErrorFields) {
			$serverErrorFields.forEach(field => field.classList.remove('invalid'));
		}
	}

	function isUsernameMatchPattern(password: string): string | null {
		if (!USERNAME_REGEX.test(password)) {
			return 'Имя пользователя может содержать только латинские буквы и арабские цифры и быть не менее 4 и не более 30 символов';
		}

		return null;
	}

	function isPasswordMatchPattern(password: string): string | null {
		if (!PASSWORD_REGEX.test(password)) {
			return 'Пароль должен содержать строчную букву, заглавную букву, цифру, символ и быть не менее 8 и не более 30 смовлов';
		}

		return null;
	}

	function isEmailMatchPattern(email: string): string | null {
		if (!EMAIL_REGEX.test(email)) {
			return 'Email не соотвествует требованиям';
		}

		return null;
	}

	function isPhoneMatchPattern(phone: string): string | null {
		// TODO: update phone validator
		if (phone.length < 10) {
			return 'Телефон не соотвествует требованиям';
		}

		return null;
	}

	function comparePasswords(): string | null {
		if (!($newPassword.value === $confirmedNewPassword.value)) {
			return 'Пароли не совпадают';
		}

		return null;
	}

	function checkForgotPasswordValidation(): void {
		const $navEmail: HTMLInputElement = document.querySelector<HTMLInputElement>('#forgot-password-nav-email');
		const $navPhone: HTMLInputElement = document.querySelector<HTMLInputElement>('#forgot-password-nav-phone');
		const $contentEmail: HTMLLabelElement = document.querySelector<HTMLLabelElement>('#forgot-password-content-email');
		const $contentPhone: HTMLLabelElement = document.querySelector<HTMLLabelElement>('#forgot-password-content-phone');
		const $inputEmail: HTMLInputElement = $contentEmail.querySelector<HTMLInputElement>('input');
		const $inputPhone: HTMLInputElement = $contentPhone.querySelector<HTMLInputElement>('input');
		const $errorMessage: HTMLDivElement = document.querySelector<HTMLDivElement>('#forgot-password-error-message');
		const $submitButton: HTMLButtonElement = document.querySelector<HTMLButtonElement>('#forgot-password-submit-button');

		if (
			$navEmail === null ||
			$navPhone === null ||
			$inputEmail === null ||
			$inputPhone === null ||
			$errorMessage === null ||
			$submitButton === null
		) {
			console.error('Not found required element in forgot password tabs');
			return;
		}

		$navEmail.addEventListener('change', () => {
			changeNav();
		});

		$navPhone.addEventListener('change', () => {
			changeNav();
		});

		$inputEmail.addEventListener('input', () => {
			checkErrors($inputEmail, isEmailMatchPattern);
		});

		$inputPhone.addEventListener('input', () => {
			checkErrors($inputPhone, isPhoneMatchPattern);
		});

		function changeNav(): void {
			if ($navEmail.checked) {
				$contentEmail.classList.add('active');
				$contentPhone.classList.remove('active');
			} else {
				$contentEmail.classList.remove('active');
				$contentPhone.classList.add('active');
			}

			$inputEmail.value = null;
			$inputEmail.classList.remove('invalid');
			$inputPhone.value = null;
			$inputPhone.classList.remove('invalid');
			$submitButton.disabled = true;
			$errorMessage.innerText = '';
			$errorMessage.classList.remove('active');
		}

		function checkErrors(input: HTMLInputElement, pattern: (value: string) => string): void {
			$submitButton.disabled = true;
			if (input.value.length > 0) {
				$errorMessage.innerText = pattern(input.value);
				if ($errorMessage.innerText) {
					$errorMessage.classList.add('active');
					input.classList.add('invalid');
				} else {
					$errorMessage.classList.remove('active');
					input.classList.remove('invalid');
					$submitButton.disabled = false;
				}
			} else {
				$errorMessage.innerText = '';
				$errorMessage.classList.remove('active');
				input.classList.remove('invalid');
			}
		}
	}

	function createDigitCodeInput(): void {
		const $digitCodeLabel: HTMLLabelElement = document.createElement('label');
		$digitCodeInput.parentNode?.insertBefore($digitCodeLabel, $digitCodeInput);

		const $digitCodeView: HTMLDivElement = document.createElement('div');
		$digitCodeView.classList.add('digit-code__view');

		for (let i = 0; i < 6; i++) {
			const $digitCodeViewItem: HTMLDivElement = document.createElement('div');
			$digitCodeViewItem.classList.add('digit-code__view-item');
			$digitCodeView.appendChild($digitCodeViewItem);
		}

		$digitCodeLabel.appendChild($digitCodeView);
		$digitCodeLabel.appendChild($digitCodeInput);

		$digitCodeInput.addEventListener('input', () => {
			updateDigitCode();
		});

		$digitCodeInput.addEventListener('focus', () => {
			updateDigitCode();
		});

		$digitCodeInput.addEventListener('blur', () => {
			for (let i = 0; i < 6; i++) {
				$digitCodeView.children[i].classList.remove('digit-code__view-item--active');
			}
		});

		function updateDigitCode(): void {
			$digitCodeInput.value = $digitCodeInput.value.replace(/\D/g, '');

			for (let i = 0; i < 6; i++) {
				$digitCodeView.children[i].innerHTML = $digitCodeInput.value[i] ? $digitCodeInput.value[i] : '';

				if (i === $digitCodeInput.value.length) {
					$digitCodeView.children[i].classList.add('digit-code__view-item--active');
				} else {
					$digitCodeView.children[i].classList.remove('digit-code__view-item--active');
				}

				$digitCodeButton.disabled = $digitCodeInput.value.length < 6;
			}
		}
	}
})();

function startResendTimer(untilResend: number): void {
	const $resendButton: HTMLButtonElement = document.querySelector<HTMLButtonElement>('#resendButton');

	if ($resendButton !== null) {
		if (untilResend <= 0) {
			$resendButton.disabled = false;
			return;
		}

		const $timer: HTMLDivElement = document.createElement('div');
		$resendButton.insertAdjacentElement('afterbegin', $timer);
		const interval: number = window.setInterval(updateText, 1000);

		function updateText(): void {
			untilResend -= 1;

			const minutes = Math.floor(untilResend / 60);
			const seconds = untilResend % 60;
			$timer.innerText = (minutes < 10 ? `0${minutes}` : minutes) + ':' + (seconds < 10 ? `0${seconds}` : seconds);

			if (untilResend <= 0) {
				$timer.remove();
				$resendButton.disabled = false;
				window.clearInterval(interval);
			}
		}
	}
}
