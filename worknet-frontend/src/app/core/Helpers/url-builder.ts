export function buildProfileImageUrl(
  profilePhotoId?: string | null,
  userName?: string | null,
  firstName?: string | null,
  lastName?: string | null,
): string {
  // Priority 1: Use profilePhotoId if available
      console.log(profilePhotoId)

  if (profilePhotoId && profilePhotoId.trim() !== '') {
    console.log("works PROFILE ID")
    return `https://drive.google.com/thumbnail?id=${profilePhotoId}`;
  }

  // Priority 2: Use firstName and lastName if both are available
  if (firstName && firstName.trim() !== '' && lastName && lastName.trim() !== '') {
    const fullName = `${firstName.trim()}+${lastName.trim()}`;
    return `https://ui-avatars.com/api/?name=${encodeURIComponent(fullName)}`;
  }

  // Priority 3: Use userName if available
  if (userName && userName.trim() !== '') {
    return `https://ui-avatars.com/api/?name=${encodeURIComponent(userName.trim())}`;
  }

  // Fallback: If no specific information is provided, return a generic placeholder or a default ui-avatar
  console.warn('buildProfileImageUrl: No specific profile information provided, using default avatar.');
  return `https://ui-avatars.com/api/?name=Uknown`;
}