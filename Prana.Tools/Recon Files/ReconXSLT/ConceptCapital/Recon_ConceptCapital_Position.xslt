<?xml version="1.0" encoding="utf-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com">

	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="Comparision">
				<xsl:if  test="COL7 != 0 and COL7 != 'Quantity' and COL1 != 'Account Number'">

					<PositionMaster>
						<!--FundName Section-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL1,'&quot;','')"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='GS']/FundData[@GSFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<FundName>
									<xsl:value-of select='$PB_FUND_NAME'/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>


						<xsl:choose>
							<xsl:when  test="COL7 &lt; 0">
								
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
								<Quantity>
									<xsl:value-of select="COL7*(-1)"/>
								</Quantity>
							</xsl:when >
							<xsl:when test="COL7 &gt; 0">								
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
								<Quantity>
									<xsl:value-of select="COL7"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>							
								<Side>
									<xsl:value-of select="''"/>
								</Side>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name = "varInstrumentType" >
							<xsl:value-of select="translate(translate(COL2, ' ' , ''),'&quot;','')"/>
						</xsl:variable>

						<!--Prana Symbol Section -->
						<xsl:choose>
							<xsl:when test ="$varInstrumentType='EQUITY'">
								<Symbol>
									<xsl:value-of select="translate(COL3,'&quot;','')"/>
								</Symbol>
							</xsl:when>
							<xsl:when test ="$varInstrumentType='OPTION'">
								<Symbol>
									<xsl:value-of select="translate(COL5,'&quot;','')"/>
								</Symbol>
							</xsl:when>
							<xsl:when test ="$varInstrumentType='FUTURE'">
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length(translate(translate(COL6,'&quot;',''),' ',''))"/>
								</xsl:variable>
								<xsl:choose>
									<xsl:when test ="$varLength &gt; 0 ">
										<xsl:variable name = "varAfter" >
											<xsl:value-of select="substring(translate(translate(COL6,'&quot;',''),' ',''),($varLength)-1,2)"/>
										</xsl:variable>
										<xsl:variable name = "varBefore" >
											<xsl:value-of select="substring(translate(translate(COL6,'&quot;',''),' ',''),1,($varLength)-2)"/>
										</xsl:variable>
										<Symbol>
											<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
										</Symbol>
									</xsl:when>
									<xsl:otherwise>
										<Symbol>
											<xsl:value-of select="''"/>
										</Symbol>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>

						<!--Prime broker Asset Type Section -->
						<PBAssetName>
							<xsl:value-of select="$varInstrumentType"/>
						</PBAssetName>

						<!--Prime broker Symbol Section -->
						<xsl:choose>
							<xsl:when test ="$varInstrumentType='EQUITY'">
								<PBSymbol>
									<xsl:value-of select="translate(COL3,'&quot;','')"/>
								</PBSymbol>
							</xsl:when>
							<xsl:when test ="$varInstrumentType='OPTION'">
								<PBSymbol>
									<xsl:value-of select="translate(COL5,'&quot;','')"/>
								</PBSymbol>
							</xsl:when>
							<xsl:when test ="$varInstrumentType='FUTURE'">
								<PBSymbol>
									<xsl:value-of select="COL6"/>
								</PBSymbol>
							</xsl:when>
							<xsl:otherwise>
								<PBSymbol>
									<xsl:value-of select="COL3"/>
								</PBSymbol>
							</xsl:otherwise>
						</xsl:choose>

						<!--Average Price Section-->
						<AvgPX>
							<xsl:value-of select="COL16"/>
						</AvgPX>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
