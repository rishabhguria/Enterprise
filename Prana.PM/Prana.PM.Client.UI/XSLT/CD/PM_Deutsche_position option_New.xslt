<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>






	<xsl:template name="tempEquityExchangeOtherThanUS">
		<xsl:param name="paramExchange"/>
		<xsl:choose>
			<xsl:when test ="$paramExchange='CAD' ">
				<xsl:value-of select ="'TC'"/>
			</xsl:when>
			<xsl:when test ="$paramExchange='EUR' ">
				<xsl:value-of select ="'FRA'"/>
			</xsl:when>
			<xsl:when test ="$paramExchange='AUD'">
				<xsl:value-of select ="'ASX'"/>
			</xsl:when>
			<xsl:when test ="$paramExchange='HKD'">
				<xsl:value-of select ="'HKG'"/>
			</xsl:when>
			<xsl:when test ="$paramExchange='GBP'">
				<xsl:value-of select ="'LON'"/>
			</xsl:when>
			<xsl:when test ="$paramExchange='JPY'">
				<xsl:value-of select ="'TSE'"/>
			</xsl:when>
			<xsl:when test ="$paramExchange='SGD'">
				<xsl:value-of select ="'SES'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Month">
		<xsl:param name="varMonth"/>
		<!-- 2 characters for month code -->
		<!-- month Codes e.g. 01 represents 01 = January-->
		<xsl:choose>
			<xsl:when test ="$varMonth='A'">
				<xsl:value-of select ="'01'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='B'">
				<xsl:value-of select ="'02'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='C'">
				<xsl:value-of select ="'03'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='D'">
				<xsl:value-of select ="'04'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='E'">
				<xsl:value-of select ="'05'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='F'">
				<xsl:value-of select ="'06'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='G'">
				<xsl:value-of select ="'07'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='H'">
				<xsl:value-of select ="'08'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='I'">
				<xsl:value-of select ="'09'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='J'">
				<xsl:value-of select ="'10'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='K'">
				<xsl:value-of select ="'11'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='L'">
				<xsl:value-of select ="'12'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="contains(COL11,'Cash')= false and contains(COL10,'T')!=false and (contains(COL3,'10602437')!=false or contains(COL3,'10606777')!=false or contains(COL3,'10602438')!=false or contains(COL3,'10606110')!=false)">
					<PositionMaster>
						<!--  Symbol Region -->
						<xsl:variable name="varSecurityType" select="COL12"/>
						<xsl:variable name="PB_COMPANY_NAME" select="normalize-space(COL22)"/>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>
											
						
						<xsl:variable name ="varExchange">
							<xsl:choose>
								<xsl:when test ="COL35 != 'USD'">
									<xsl:call-template name="tempEquityExchangeOtherThanUS">
										<xsl:with-param name="paramExchange" select="COL35" />
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name ="varSymbol">
							<xsl:choose>
								<xsl:when test ="normalize-space(COL11)= 'B' ">
									<xsl:value-of select ="COL16"/>
								</xsl:when>

								<xsl:when test ="contains(COL14,'.TO')!=False and normalize-space(COL11)!= 'B' ">
									<xsl:value-of select ="concat(substring-before(COL14,'.'),'-TC')"/>
								</xsl:when>
								<xsl:when test ="contains(COL14,'.TO')=False and normalize-space(COL11)!= 'B'">
									<xsl:value-of select ="substring-before(COL14,'.')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name ="varPBSymbol">
							<xsl:choose>

								<xsl:when test ="contains($varSymbol,'_')!=False ">
									<xsl:value-of select ="concat(substring-before($varSymbol,'_'),'/',translate(substring-after($varSymbol,'_'),$vLowercaseChars_CONST,$vUppercaseChars_CONST))"/>
								</xsl:when>
								<xsl:when test ="contains($varSymbol,'_')=False">
									<xsl:value-of select ="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="varPBSymbol_Length" select="string-length($varPBSymbol)"/>
						<xsl:variable name="varOption_Underlying" select="substring($varPBSymbol,1,$varPBSymbol_Length -10)"/>
						<xsl:variable name="varOption_Year" select="substring($varPBSymbol,$varPBSymbol_Length -6,2)"/>
						<xsl:variable name="varMonthCode" select="substring($varPBSymbol,$varPBSymbol_Length -9,1)"/>
						<xsl:variable name = "varOption_ExpirationMonth" >
							<xsl:call-template name="Month">
								<xsl:with-param name="varMonth" select="$varMonthCode" />
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="varOption_StrikePrice" select="concat(number(substring($varPBSymbol,$varPBSymbol_Length -4,3)),'.',substring($varPBSymbol,$varPBSymbol_Length -1,2))"/>
						<Description>
							<xsl:value-of select="COL22"/>
						</Description>

						<!--<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME!=''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>
								
								<PBSymbol>
									<xsl:value-of select="COL22"/>
								</PBSymbol>
								
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:when>
							<xsl:when test ="COL35='USD'">
								<Symbol>
									<xsl:value-of select="COL15"/>
								</Symbol>
								
								<PBSymbol>
									<xsl:value-of select="COL15"/>
								</PBSymbol>
								
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:when>
							<xsl:when test ="COL35 !='USD'">
								<Symbol>
									<xsl:value-of select="concat(substring-before(COL14,'.'),'-',$varExchange)"/>
								</Symbol>
								
								<PBSymbol>
									<xsl:value-of select="COL14"/>
								</PBSymbol>
								
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:when>														
						</xsl:choose>-->

						<xsl:choose>
							<xsl:when test ="$varSecurityType='Equity Option'">
								<Symbol>
									<xsl:value-of select ="concat('O:',$varOption_Underlying,' ',$varOption_Year,$varMonthCode,$varOption_StrikePrice)"/>
								</Symbol>

								
							</xsl:when>
							
							<xsl:when test ="$varSecurityType !='Equity Option'">
								<Symbol>
									<xsl:value-of select ="$varPBSymbol"/>
								</Symbol>
								
							</xsl:when>
							
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								
							</xsl:otherwise>
						</xsl:choose>
						
						<PBSymbol>
							<xsl:value-of select="COL22"/>
						</PBSymbol>

						<xsl:choose>
							<xsl:when test="COL26 &lt; 0">
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="COL26 &gt; 0">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:otherwise>
								<SideTagValue>
									<xsl:value-of select="''"/>
								</SideTagValue>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="COL26 &lt; 0">
								<NetPosition>
									<xsl:value-of select="COL26*(-1)"/>
								</NetPosition>
							</xsl:when>
							<xsl:when test="COL26 &gt; 0">
								<NetPosition>
									<xsl:value-of select="COL26"/>
								</NetPosition>
							</xsl:when>
							<xsl:otherwise>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="boolean(number(COL29))">
								<CostBasis>
									<xsl:value-of select="COL29"/>
								</CostBasis>
							</xsl:when>														
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>	
						
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL3"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='GS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<xsl:choose>
							
						<xsl:when test="$PRANA_FUND_NAME=''">
							<AccountName>
								<xsl:value-of select='$PB_FUND_NAME'/>
							</AccountName>
						</xsl:when>
						<xsl:otherwise>
							<AccountName>
								<xsl:value-of select='$PRANA_FUND_NAME'/>
							</AccountName>
						</xsl:otherwise>
    					</xsl:choose>
						
						<PositionStartDate>
							<xsl:value-of select="''"/>
						</PositionStartDate>							
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
