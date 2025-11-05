<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:my="put-your-namespace-uri-here">

	<xsl:output method="xml" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="varAsset">
					<xsl:value-of select="normalize-space(COL11)"/>
				</xsl:variable>

				<xsl:variable name="varAssetType">
					<xsl:choose>
						<xsl:when test="substring-before(normalize-space(COL11),' ') = 'CALL'  or substring-before(normalize-space(COL11),' ') = 'PUT'">
							<xsl:value-of select="'EquityOption'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'Equity'"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>


				<xsl:if test="$varAssetType='Equity' and COL1 !='CCY'">

					<PositionMaster>

						<xsl:variable name="varTicker">
							<xsl:value-of select="normalize-space(COL15)"/>
						</xsl:variable>

						<TickerSymbol>
							<xsl:value-of select="$varTicker"/>
						</TickerSymbol>

						<UnderLyingSymbol>
							<xsl:value-of select="$varTicker"/>
						</UnderLyingSymbol>

						<xsl:variable name="varISIN">
							<xsl:value-of select="normalize-space(COL12)"/>
						</xsl:variable>

						<ISINSymbol>
							<xsl:choose>
								<xsl:when test="$varISIN!=''">
									<xsl:value-of select="$varISIN"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</ISINSymbol>

						<xsl:variable name="varCusip">
							<xsl:value-of select="normalize-space(COL14)"/>
						</xsl:variable>

						<CusipSymbol>
							<xsl:choose>
								<xsl:when test="$varCusip!=''">
									<xsl:value-of select="$varCusip"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</CusipSymbol>

						<xsl:variable name="varSedol">
							<xsl:value-of select="normalize-space(COL13)"/>
						</xsl:variable>

						<SedolSymbol>
							<xsl:choose>
								<xsl:when test="$varSedol!=''">
									<xsl:value-of select="$varSedol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SedolSymbol>

						<LongName>
							<xsl:value-of select="normalize-space(COL11)"/>
						</LongName>

						<Multiplier>
							<xsl:value-of select="1"/>
						</Multiplier>

						<AUECID>
							<xsl:value-of select="1"/>
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