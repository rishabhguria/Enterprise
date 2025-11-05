<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="DocumentElement">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:variable name = "varCADFXPrice">
				<xsl:value-of select="PositionMaster[translate(COL31,' ','')='CAD_CCY'][COL28 != 0]/COL28"/>
			</xsl:variable>
			<xsl:variable name = "varAUDFXPrice">
				<xsl:value-of select="PositionMaster[translate(COL31,' ','')='AUD_CCY'][COL28 != 0]/COL28"/>
			</xsl:variable>
			<xsl:variable name = "varGBPFXPrice">
				<xsl:value-of select="PositionMaster[translate(COL31,' ','')='GBP_CCY'][COL28 != 0]/COL28"/>
			</xsl:variable>
			<xsl:variable name = "varEURFXPrice">
				<xsl:value-of select="PositionMaster[translate(COL31,' ','')='EUR_CCY'][COL28 != 0]/COL28"/>
			</xsl:variable>
			<xsl:variable name = "varINRFXPrice">
				<xsl:value-of select="PositionMaster[translate(COL31,' ','')='INR_CCY'][COL28 != 0]/COL28"/>
			</xsl:variable>

			<xsl:for-each select="PositionMaster">
				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="translate(COL11,'&quot;','')"/>
				</xsl:variable>

				<xsl:if test="$varInstrumentType='50' or $varInstrumentType='60' or $varInstrumentType='70'">
					<PositionMaster>
						<xsl:variable name ="varCurr">
							<xsl:value-of select="translate(COL31,' ','')"/>
						</xsl:variable>

						<!--<xsl:value-of select="'USD'"/>-->
						<xsl:choose>
							<xsl:when test ="$varCurr='CAD_CCY'">
								<BaseCurrency>
									<xsl:value-of select ="'CAD'"/>
								</BaseCurrency>
							</xsl:when>
							<xsl:when test ="$varCurr='AUD_CCY'">
								<BaseCurrency>
									<xsl:value-of select ="'AUD'"/>
								</BaseCurrency>
							</xsl:when>
							<xsl:when test ="$varCurr='EUR_CCY'">
								<BaseCurrency>
									<xsl:value-of select ="'EUR'"/>
								</BaseCurrency>
							</xsl:when>
							<xsl:when test ="$varCurr='GBP_CCY'">
								<BaseCurrency>
									<xsl:value-of select ="'GBP'"/>
								</BaseCurrency>
							</xsl:when>
							<xsl:otherwise>
								<BaseCurrency>
									<xsl:value-of select ="'USD'"/>
								</BaseCurrency>
							</xsl:otherwise>
						</xsl:choose>

						<SettlementCurrency>
							<xsl:value-of select="'USD'"/>
							<!--<xsl:value-of select="translate(COL44,'&quot;','')"/>-->
						</SettlementCurrency>

						<!--<xsl:choose>
							<xsl:when test ="varCurr = 'CAD_CCY'">
								<ForexPrice>
									<xsl:value-of select="$varCADFXPrice"/>
								</ForexPrice>								
							</xsl:when >
							<xsl:when test ="boolean(number(COL28)) and varCurr = 'AUD_CCY'">
								<ForexPrice>
									<xsl:value-of select="$varAUDFXPrice"/>
								</ForexPrice>
							</xsl:when >
							<xsl:when test ="boolean(number(COL28)) and varCurr = 'GBP_CCY'">
								<ForexPrice>
									<xsl:value-of select="$varGBPFXPrice"/>
								</ForexPrice>
							</xsl:when >
							<xsl:when test ="boolean(number(COL28)) and varCurr = 'EUR_CCY'">
								<ForexPrice>
									<xsl:value-of select="$varEURFXPrice"/>
								</ForexPrice>
							</xsl:when >
							<xsl:when test ="boolean(number(COL28)) and varCurr = 'INR_CCY'">
								<ForexPrice>
									<xsl:value-of select="$varINRFXPrice"/>
								</ForexPrice>
							</xsl:when >
							<xsl:otherwise>
								<ForexPrice>
									<xsl:value-of select="0"/>
								</ForexPrice>
							</xsl:otherwise>
						</xsl:choose >-->
						<xsl:choose>
							<xsl:when test ="boolean(number(COL28)) and $varCurr='CAD_CCY'">
								<ForexPrice>
									<xsl:value-of select="$varCADFXPrice"/>
								</ForexPrice>
							</xsl:when >
							<xsl:when test ="boolean(number(COL28)) and $varCurr='AUD_CCY'">
								<ForexPrice>
									<xsl:value-of select="$varAUDFXPrice"/>
								</ForexPrice>
							</xsl:when >
							<xsl:when test ="boolean(number(COL28)) and $varCurr='GBP_CCY'">
								<ForexPrice>
									<xsl:value-of select="$varGBPFXPrice"/>
								</ForexPrice>
							</xsl:when >
							<xsl:when test ="boolean(number(COL28)) and $varCurr='EUR_CCY'">
								<ForexPrice>
									<xsl:value-of select="$varEURFXPrice"/>
								</ForexPrice>
							</xsl:when >
							<xsl:when test ="boolean(number(COL28)) and $varCurr='INR_CCY'">
								<ForexPrice>
									<xsl:value-of select="$varINRFXPrice"/>
								</ForexPrice>
							</xsl:when >
							<xsl:otherwise>
								<ForexPrice>
									<xsl:value-of select="0"/>
								</ForexPrice>
							</xsl:otherwise>
						</xsl:choose >

						<xsl:variable name = "varYR" >
							<xsl:value-of select="translate(substring(COL4,1,4),'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name = "varMth" >
							<xsl:value-of select="translate(substring(COL4,5,2),'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name = "varDt" >
							<xsl:value-of select="translate(substring(COL4,7,2),'&quot;','')"/>
						</xsl:variable>
						<Date>
							<xsl:value-of select="translate(concat($varYR,'/',$varMth,'/',$varDt),'&quot;','')"/>
						</Date>

						<!--<FXConversionMethodOperator>
							<xsl:value-of select ="COL47"/>
						</FXConversionMethodOperator>-->
					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
