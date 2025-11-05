<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>				
			<xsl:for-each select="//PositionMaster">
				<xsl:if test ="translate(COL1,'&quot;','')='CASH AND EQUIVALENTS'">	
					<PositionMaster>
						<FundName>
							<xsl:value-of select="''"/>
						</FundName>
						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>						
						<LocalCurrency>
							<xsl:value-of select="translate(COL3,'&quot;','')"/>
						</LocalCurrency>
						
						<xsl:variable name = "varLocalCashValIstChar">
							<xsl:value-of select="substring(COL13,1,1)"/>
						</xsl:variable>	
						<xsl:choose>
							<xsl:when test ="$varLocalCashValIstChar='('">
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length(COL13)"/>
								</xsl:variable>								
								<xsl:variable name = "varSymbolWithoutBraces" >
									<xsl:value-of select="substring(COL13,2,($varLength)-2)"/>
								</xsl:variable>
								<CashValueLocal>
									<xsl:value-of select="$varSymbolWithoutBraces * (-1)"/>
								</CashValueLocal>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test ="COL13 &lt; 0 or COL13 &gt; 0 or COL13 = 0">
										<CashValueLocal>
											<xsl:value-of select="COL13"/>
										</CashValueLocal>
									</xsl:when >
									<xsl:otherwise>
										<CashValueLocal>
											<xsl:value-of select="0"/>
										</CashValueLocal>
									</xsl:otherwise>
								</xsl:choose >
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name = "varBaseCashValIstChar">
							<xsl:value-of select="substring(COL15,1,1)"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test ="$varBaseCashValIstChar='('">
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length(COL15)"/>
								</xsl:variable>
								<xsl:variable name = "varSymbolWithoutBraces" >
									<xsl:value-of select="substring(COL15,2,($varLength)-2)"/>
								</xsl:variable>
								<CashValueBase>
									<xsl:value-of select="$varSymbolWithoutBraces * (-1)"/>
								</CashValueBase>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test ="COL15 &lt; 0 or COL15 &gt; 0 or COL15 = 0">
										<CashValueBase>
											<xsl:value-of select="COL15"/>
										</CashValueBase>
									</xsl:when >
									<xsl:otherwise>
										<CashValueBase>
											<xsl:value-of select="0"/>
										</CashValueBase>
									</xsl:otherwise>
								</xsl:choose >
							</xsl:otherwise>
						</xsl:choose>
						
						
											
						<PositionType>
							<xsl:value-of select="'Cash'"/>
						</PositionType>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>	
</xsl:stylesheet>
