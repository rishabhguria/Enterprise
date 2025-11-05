<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:variable name = "varCADMarkPrice">
				<xsl:value-of select="PositionMaster[COL28='CAD'][COL29!=0]/COL29"/>
			</xsl:variable>
			<xsl:variable name = "varAUDMarkPrice">
				<xsl:value-of select="PositionMaster[COL28='AUD'][COL29!=0]/COL29"/>
			</xsl:variable>
			<xsl:variable name = "varGBPMarkPrice">
				<xsl:value-of select="PositionMaster[COL28='GBp'][COL29!=0]/COL29"/>
			</xsl:variable>
			<xsl:variable name = "varEURMarkPrice">
				<xsl:value-of select="PositionMaster[COL28='EUR'][COL29!=0]/COL29"/>
			</xsl:variable>
			<xsl:variable name = "varINRMarkPrice">
				<xsl:value-of select="PositionMaster[COL28='INR'][COL29!=0]/COL29"/>
			</xsl:variable>

			<xsl:for-each select="PositionMaster">

				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="translate(COL11,'&quot;','')"/>
				</xsl:variable>

				<xsl:if test="$varInstrumentType='50' or $varInstrumentType='60' or $varInstrumentType='70'">
					<PositionMaster>
						<xsl:variable name="PB_COMPANY_NAME" select="translate(COL13,'&quot;','')"/>
						<!--CompanyName and Symbol Section-->
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='BOFA']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<!--<xsl:choose>
							<xsl:when test="($varInstrumentType='50' or $varInstrumentType='70') and $PRANA_SYMBOL_NAME != ''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>
							</xsl:when>
							<xsl:when test="$varInstrumentType='60'">
								-->
						<!-- QPXPHO-->
						<!--
								<xsl:variable name="varAfterQ" >
									<xsl:value-of select="substring-after(COL5,'Q')"/>
								</xsl:variable>
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length($varAfterQ)"/>
								</xsl:variable>
								<xsl:variable name = "varAfter" >
									<xsl:value-of select="substring($varAfterQ,($varLength)-1,2)"/>
								</xsl:variable>
								<xsl:variable name = "varBefore" >
									<xsl:value-of select="substring($varAfterQ,1,($varLength)-2)"/>
								</xsl:variable>
								<Symbol>
									<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
								</Symbol>
							</xsl:when >
							<xsl:otherwise>
								<xsl:variable name="varAfterAstric" >
									<xsl:value-of select="substring-after(COL5,'*')"/>
								</xsl:variable>
								<xsl:choose>
									<xsl:when test ="$varAfterAstric =''">
										<Symbol>
											<xsl:value-of select="COL5"/>
										</Symbol>
									</xsl:when>
									<xsl:otherwise>
										<Symbol>
											<xsl:value-of select="$varAfterAstric"/>
										</Symbol>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>-->

						<!--<xsl:choose>
							<xsl:when  test="COL21 != 0 and COL29 != 0 ">
								<MarkPrice>
									<xsl:value-of select="COL21 * COL29"/>
								</MarkPrice>
							</xsl:when >
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="COL21"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose >-->
						<xsl:variable name ="varSymbol">
							<xsl:choose>
								<xsl:when test="($varInstrumentType='50' or $varInstrumentType='70') and $PRANA_SYMBOL_NAME != ''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varInstrumentType='60'">
									<!-- QPXPHO-->
									<xsl:variable name="varAfterQ" >
										<xsl:value-of select="substring-after(COL5,'Q')"/>
									</xsl:variable>
									<xsl:variable name = "varLength" >
										<xsl:value-of select="string-length($varAfterQ)"/>
									</xsl:variable>
									<xsl:variable name = "varAfter" >
										<xsl:value-of select="substring($varAfterQ,($varLength)-1,2)"/>
									</xsl:variable>
									<xsl:variable name = "varBefore" >
										<xsl:value-of select="substring($varAfterQ,1,($varLength)-2)"/>
									</xsl:variable>

									<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:variable name="varAfterAstric" >
										<xsl:value-of select="substring-after(COL5,'*')"/>
									</xsl:variable>
									<xsl:choose>
										<xsl:when test ="$varAfterAstric =''">
											<xsl:value-of select="COL5"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$varAfterAstric"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Symbol>
							<xsl:value-of select ="$varSymbol"/>
						</Symbol>

						<xsl:variable name ="varCheckSymbolUnderlying">
							<xsl:value-of select ="substring-before($varSymbol,'-')"/>
						</xsl:variable>


						<xsl:choose>
							<xsl:when  test="COL28 = 'CAD' and $varCheckSymbolUnderlying != ''">
								<MarkPrice>
									<xsl:value-of select ="COL21 * $varCADMarkPrice"/>
								</MarkPrice>
							</xsl:when >
							<xsl:when  test="COL28 = 'AUD' and $varCheckSymbolUnderlying != ''">
								<MarkPrice>
									<xsl:value-of select ="COL21 * $varAUDMarkPrice"/>
								</MarkPrice>
							</xsl:when >
							<xsl:when  test="COL28 = 'GBp' and $varCheckSymbolUnderlying != ''">
								<MarkPrice>
									<xsl:value-of select ="COL21 * $varGBPMarkPrice"/>
								</MarkPrice>
							</xsl:when >
							<xsl:when  test="COL28 = 'EUR' and $varCheckSymbolUnderlying != ''">
								<MarkPrice>
									<xsl:value-of select ="COL21 * $varEURMarkPrice"/>
								</MarkPrice>
							</xsl:when >
							<xsl:when  test="COL28 = 'INR' and $varCheckSymbolUnderlying != ''">
								<MarkPrice>
									<xsl:value-of select ="COL21 * $varINRMarkPrice"/>
								</MarkPrice>
							</xsl:when >
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="COL21"/>
								</MarkPrice>
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

					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>
