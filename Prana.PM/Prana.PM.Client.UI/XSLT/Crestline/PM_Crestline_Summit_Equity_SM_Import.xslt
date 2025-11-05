<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:my="put-your-namespace-uri-here">

	<xsl:output method="xml" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="varAsset">
					<xsl:value-of select="normalize-space(COL55)"/>
				</xsl:variable>

				<xsl:variable name="varAssetType">
					<xsl:choose>
						<xsl:when test="$varAsset = 'Option'">
							<xsl:value-of select="'EquityOption'"/>
						</xsl:when>
						<xsl:when test="$varAsset = 'Equity'">
							<xsl:value-of select="'Equity'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="''"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>


				<xsl:if test="$varAssetType='Equity'">

					<PositionMaster>

						<xsl:variable name="varTicker">
							<xsl:value-of select="substring-before(normalize-space(COL7),' ')"/>
						</xsl:variable>

						<TickerSymbol>
							<xsl:value-of select="$varTicker"/>
						</TickerSymbol>

						<UnderLyingSymbol>
							<xsl:value-of select="$varTicker"/>
						</UnderLyingSymbol>
						
						<xsl:variable name="varBloomberg">
							<xsl:value-of select="normalize-space(COL7)"/>
						</xsl:variable>

						<BloombergSymbol>
							<xsl:choose>
								<xsl:when test="$varBloomberg !=''">
									<xsl:value-of select="$varBloomberg"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</BloombergSymbol>

						<LongName>
							<xsl:value-of select="COL8"/>
						</LongName>

						<Multiplier>
							<xsl:value-of select="1"/>
						</Multiplier>

						<AUECID>
							<xsl:value-of select="'1'"/>
						</AUECID>

						<UDASector>
							<xsl:value-of select="'Undefined'"/>
						</UDASector>

						<UDASubSector>
							<xsl:value-of select="'Undefined'"/>
						</UDASubSector>

						<UDASecurityType>
							<xsl:value-of select="'Undefined'"/>
						</UDASecurityType>

						<UDAAssetClass>
							<xsl:value-of select="'Undefined'"/>
						</UDAAssetClass>

						<UDACountry>
							<xsl:value-of select="'Undefined'"/>
						</UDACountry>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>