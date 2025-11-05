<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.contoso.com">
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

	<xsl:template name="noofzeros">
		<xsl:param name="count"/>
		<xsl:if test="$count > 0">
			<xsl:value-of select ="'0'"/>
			<xsl:call-template name="noofzeros">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="noofBlanks">
		<xsl:param name="count1"/>
		<xsl:if test="$count1 > 0">
			<xsl:value-of select ="' '"/>
			<xsl:call-template name="noofBlanks">
				<xsl:with-param name="count1" select="$count1 - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>



		<xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision">
		  <xsl:variable name="varSecurity_Type" select="normalize-space(substring(COL1,25,2))"/>
		  <xsl:variable name="varTransaction_type" select="normalize-space(substring(COL1,15,2))"/>
		  <xsl:if test="($varTransaction_type='by' or $varTransaction_type='cs' or $varTransaction_type='sl' or $varTransaction_type='ss') and ($varSecurity_Type='cl' or $varSecurity_Type='cs' or $varSecurity_Type='pt')">

			  <PositionMaster>

            <!--   Fund -->
			<xsl:variable name = "PB_FUND_NAME" >
				<xsl:value-of select="substring(COL1,1,14)"/>
			</xsl:variable>
			<xsl:variable name="PRANA_FUND_NAME">
				<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SUNGARD']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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
			</xsl:choose >

			
				<xsl:variable name="varPBSymbol" select="translate(normalize-space(substring(COL1,28,9)),$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
				<xsl:variable name="varOptionString" select="translate(normalize-space(substring(COL1,127,30)),$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
				<xsl:variable name="varFirst_StringofLength6" select="substring($varOptionString,1,6)"/>
				<xsl:variable name="varRemainigString" select="substring($varOptionString,7)"/>

				<!--String Part Before C/P-->
				<xsl:variable name ="varStringBeforCallPutcode">
					<xsl:choose>
						<xsl:when test ="contains($varRemainigString,'C') != false">
							<xsl:value-of select ="substring-before($varRemainigString,'C')"/>
						</xsl:when>
						<xsl:when test ="contains($varRemainigString,'P')!= false ">
							<xsl:value-of select ="substring-before($varRemainigString,'P')"/>
						</xsl:when>
					</xsl:choose>
				</xsl:variable>

				<!--String Part After C/P (Strike price)-->
				<xsl:variable name ="varStrike">
					<xsl:choose>
						<xsl:when test ="contains($varRemainigString,'C')!= false">
							<xsl:value-of select ="substring-after($varRemainigString,'C')"/>
						</xsl:when>
						<xsl:when test ="contains($varRemainigString,'P')!= false">
							<xsl:value-of select ="substring-after($varRemainigString,'P')"/>
						</xsl:when>
					</xsl:choose>
				</xsl:variable>

				<!--Code to know about Option-->
				<xsl:variable name ="varCallPutCode">
					<xsl:choose>
						<xsl:when test ="normalize-space(substring(COL1,25,2))='cl'">
							<xsl:value-of select ="'1'"/>
						</xsl:when>
						<xsl:when test ="normalize-space(substring(COL1,25,2))='pt'">
							<xsl:value-of select ="'0'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="''"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="varUnderlyingDate" select="concat($varFirst_StringofLength6,$varStringBeforCallPutcode)" />
				<xsl:variable name="varUnderlyingDate_Length" select="string-length($varUnderlyingDate)" />
				<xsl:variable name="varOptionDate" select="substring($varUnderlyingDate,$varUnderlyingDate_Length -5)" />

				<xsl:variable name ="varRoot">
					<xsl:choose>
						<xsl:when test ="$varCallPutCode != ''">
							<xsl:value-of select ="substring($varUnderlyingDate,1,$varUnderlyingDate_Length -6)"/>
						</xsl:when>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name = "BlankCount_Root" >
					<xsl:call-template name="noofBlanks">
						<xsl:with-param name="count1" select="(6) - string-length($varRoot)" />
					</xsl:call-template>
				</xsl:variable>

				<xsl:variable name ="varStrikeIntPart">
					<xsl:choose>
						<xsl:when test ="contains($varStrike,'.')!= false">
							<xsl:value-of select ="substring-before($varStrike,'.')"/>
						</xsl:when>

						<xsl:otherwise>
							<xsl:value-of select ="$varStrike"/>
						</xsl:otherwise>

					</xsl:choose>
				</xsl:variable>

				<xsl:variable name ="varStrikeDecimalPart">
					<xsl:choose>
						<xsl:when test ="contains($varStrike,'.')!= false">
							<xsl:value-of select ="substring-after($varStrike,'.')"/>
						</xsl:when>

						<xsl:otherwise>
							<xsl:value-of select ="''"/>
						</xsl:otherwise>

					</xsl:choose>
				</xsl:variable>

				<xsl:variable name ="varStrikeInt">
					<xsl:choose>
						<xsl:when test ="$varStrikeIntPart != '' and string-length($varStrikeIntPart) = 1">
							<xsl:value-of select ="concat('0000',$varStrikeIntPart)"/>
						</xsl:when>
						<xsl:when test ="$varStrikeIntPart != '' and string-length($varStrikeIntPart) = 2">
							<xsl:value-of select ="concat('000',$varStrikeIntPart)"/>
						</xsl:when>
						<xsl:when test ="$varStrikeIntPart != '' and string-length($varStrikeIntPart) = 3">
							<xsl:value-of select ="concat('00',$varStrikeIntPart)"/>
						</xsl:when>
						<xsl:when test ="$varStrikeIntPart != '' and string-length($varStrikeIntPart) = 4">
							<xsl:value-of select ="concat('0',$varStrikeIntPart)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="$varStrikeIntPart"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name ="varStrikeDecimal">
					<xsl:choose>
						<xsl:when test ="$varStrikeDecimalPart != '' and string-length($varStrikeDecimalPart) = 1">
							<xsl:value-of select ="concat($varStrikeDecimalPart,'00')"/>
						</xsl:when>
						<xsl:when test ="$varStrikeDecimalPart != '' and string-length($varStrikeDecimalPart) = 2">
							<xsl:value-of select ="concat($varStrikeDecimalPart,'0')"/>
						</xsl:when>
						<xsl:when test ="$varStrikeDecimalPart != '' and string-length($varStrikeDecimalPart) = 3">
							<xsl:value-of select ="$varStrikeDecimalPart"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="substring($varStrikeDecimalPart,1,3)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="Strike">
					<xsl:choose>
						<xsl:when test="$varCallPutCode !='' and contains($varStrike,'.')!= false">
							<xsl:value-of select ="concat($varStrikeInt,$varStrikeDecimal)"/>
						</xsl:when>

						<xsl:otherwise>
							<xsl:value-of select ="concat($varStrikeInt,'000')"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test ="$varCallPutCode!=''">
						<Symbol>
							<xsl:value-of select="''"/>
						</Symbol>
						<IDCOOptionSymbol>
							<xsl:value-of select="concat($varRoot,$BlankCount_Root,$varOptionDate,'C',$Strike,'U')"/>
						</IDCOOptionSymbol>
						<CUSIP>
							<xsl:value-of select ="''"/>
						</CUSIP>
					</xsl:when>
					<xsl:when test ="string-length($varPBSymbol) &gt; 6">
						<Symbol>
							<xsl:value-of select="''"/>
						</Symbol>
						<IDCOOptionSymbol>
							<xsl:value-of select="''"/>
						</IDCOOptionSymbol>
						<CUSIP>
							<xsl:value-of select ="$varPBSymbol"/>
						</CUSIP>
					</xsl:when>
					<xsl:otherwise>
						<Symbol>
							<xsl:value-of select="$varPBSymbol"/>
						</Symbol>
						<IDCOOptionSymbol>
							<xsl:value-of select="''"/>
						</IDCOOptionSymbol>
						<CUSIP>
							<xsl:value-of select ="''"/>
						</CUSIP>
					</xsl:otherwise>
				</xsl:choose>


				  <xsl:choose>
					  <xsl:when test ="$varCallPutCode!=''">
						  <PBSymbol>
							  <xsl:value-of select ="translate(normalize-space(substring(COL1,127,30)),$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
						  </PBSymbol>
					  </xsl:when>
					  <xsl:otherwise>
						  <PBSymbol>
							  <xsl:value-of select="translate(normalize-space(substring(COL1,28,9)),$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
						  </PBSymbol>
					  </xsl:otherwise>
				  </xsl:choose>

				  <PBAssetName>
					  <xsl:value-of select="''"/>
				  </PBAssetName>

				  <xsl:variable name ="varSide">
					  <xsl:value-of select="substring(COL1,15,2)"/>
				  </xsl:variable>
				  <xsl:choose>
					  <xsl:when test="$varSide = 'by'  and $varCallPutCode=''">
						  <Side>
							  <xsl:value-of select="'Buy'"/>
						  </Side>
					  </xsl:when>
					  <xsl:when test="$varSide = 'ss'  and $varCallPutCode=''">
						  <Side>
							  <xsl:value-of select="'Sell'"/>
						  </Side>
					  </xsl:when>

					  <xsl:when test="$varSide = 'by'  and $varCallPutCode != ''">
						  <Side>
							  <xsl:value-of select="'Buy to open'"/>
						  </Side>
					  </xsl:when>
					  <xsl:when test="$varSide = 'ss'  and $varCallPutCode != ''">
						  <Side>
							  <xsl:value-of select="'Sell to close'"/>
						  </Side>
					  </xsl:when>

					  <xsl:otherwise>
						  <Side>
							  <xsl:value-of select="''"/>
						  </Side>
					  </xsl:otherwise>
				  </xsl:choose>

				  <xsl:choose>
					  <xsl:when  test="number(normalize-space(substring(COL1,57,11)))and normalize-space(substring(COL1,15,2))='ss'">
						  <Quantity>
							  <xsl:value-of select="normalize-space(substring(COL1,57,11))* -1"/>
						  </Quantity>
					  </xsl:when>
					  <xsl:when  test="number(normalize-space(substring(COL1,57,11)))">
						  <Quantity>
							  <xsl:value-of select="normalize-space(substring(COL1,57,11))"/>
						  </Quantity>
					  </xsl:when>
					  <xsl:otherwise>
						  <Quantity>
							  <xsl:value-of select="0"/>
						  </Quantity>
					  </xsl:otherwise>
				  </xsl:choose>

				  <xsl:choose>
					  <xsl:when test="boolean(number(normalize-space(substring(COL1,38,10))))">
						  <AvgPX>
							  <xsl:value-of select="normalize-space(substring(COL1,38,10))"/>
						  </AvgPX>
					  </xsl:when>
					  <xsl:otherwise>
						  <AvgPX>
							  <xsl:value-of select="0"/>
						  </AvgPX>
					  </xsl:otherwise>
				  </xsl:choose>
				  
				  <SMRequest>
				  <xsl:value-of select ="'TRUE'"/>
				  </SMRequest>



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
