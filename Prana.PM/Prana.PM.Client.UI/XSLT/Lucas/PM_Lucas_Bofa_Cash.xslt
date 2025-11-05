<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>	
			
			<xsl:for-each select="PositionMaster">
				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="translate(COL16,'&quot;','')"/>
				</xsl:variable>
				<xsl:variable name = "varCurrLen" >
					<xsl:value-of select="string-length(translate(COL4,'&quot;',''))"/>
				</xsl:variable>
				<xsl:if test="$varInstrumentType = '1' and $varCurrLen = 3">
					<PositionMaster>						
						<FundName>
							<xsl:value-of select="' '"/>
						</FundName>	
						
						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>
						
						<LocalCurrency>
							<xsl:value-of select="translate(COL4,'&quot;','')"/>
						</LocalCurrency>

						<!--<xsl:if test="COL11 &lt; 0">							
							<CashValueLocal>
								<xsl:value-of select="COL11*(-1)"/>
							</CashValueLocal>
						</xsl:if>

						<xsl:if test="COL11 &gt; 0">							
							<CashValueLocal>
								<xsl:value-of select="COL11"/>
							</CashValueLocal>
						</xsl:if>-->
						<xsl:choose>
							<xsl:when test ="COL11 &lt; 0 or COL11 &gt; 0 or COL11 = 0">
								<CashValueLocal>
									<xsl:value-of select="COL11"/>
								</CashValueLocal>
							</xsl:when >
							<xsl:otherwise>
								<CashValueLocal>
									<xsl:value-of select="0"/>
								</CashValueLocal>
							</xsl:otherwise>
						</xsl:choose >


						<!-- Position Date mapped with the column 10 -->
						<Date>
							<xsl:value-of select="''"/>
						</Date>
						<PositionType>
							<xsl:value-of select="COL2"/>
						</PositionType>
					</PositionMaster>
				</xsl:if>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>
