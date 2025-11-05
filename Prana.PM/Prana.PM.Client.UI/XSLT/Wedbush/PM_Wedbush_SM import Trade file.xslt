<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<!--Third Friday check-->
	<msxsl:script language="C#" implements-prefix="my">
		public string Now(int year, int month)
		{
		DateTime thirdFriday= new DateTime(year, month, 15);
		while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
		{
		thirdFriday = thirdFriday.AddDays(1);
		}
		return thirdFriday.ToString();
		}
	</msxsl:script>

	<xsl:template name="MonthNo">
		<xsl:param name="varMonth"/>

		<xsl:choose>
			<xsl:when test ="$varMonth= 'A' or $varMonth= 'M'">
				<xsl:value-of select ="1"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'B' or $varMonth= 'N'">
				<xsl:value-of select ="2"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'C' or $varMonth= 'O'">
				<xsl:value-of select ="3"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'D' or $varMonth= 'P'">
				<xsl:value-of select ="4"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'E' or $varMonth= 'Q'">
				<xsl:value-of select ="5"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'F' or $varMonth= 'R'">
				<xsl:value-of select ="6"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'G' or $varMonth= 'S'">
				<xsl:value-of select ="7"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'H' or $varMonth= 'T'">
				<xsl:value-of select ="8"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'I' or $varMonth= 'U'">
				<xsl:value-of select ="9"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'J' or $varMonth= 'V'">
				<xsl:value-of select ="10"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'K' or $varMonth= 'W'">
				<xsl:value-of select ="11"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'L' or $varMonth= 'X'">
				<xsl:value-of select ="12"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="PutorCall">
		<xsl:param name="MonthCode"/>

		<xsl:choose>
			<xsl:when test ="$MonthCode='A' or $MonthCode='B' or $MonthCode='C' or $MonthCode='D' or $MonthCode='E' or $MonthCode='F' or $MonthCode='G' or $MonthCode='H' or $MonthCode='I' or $MonthCode='J' or $MonthCode='K' or $MonthCode='L'">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when test ="$MonthCode='M' or $MonthCode='N' or $MonthCode='O' or $MonthCode='P' or $MonthCode='Q' or $MonthCode='R' or $MonthCode='S' or $MonthCode='T' or $MonthCode='U' or $MonthCode='V' or $MonthCode='W' or $MonthCode='X'">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="ConvertBBCodetoTicker">
		<xsl:param name="varBBCode"/>

		<xsl:variable name="varRoot">
			<xsl:choose>
          <xsl:when test="substring(COL5,2,1) = '1'">
            <xsl:value-of select="substring-before(COL5,'1')"/>
          </xsl:when>
          <xsl:when test="substring(COL5,2,1) = '2'">
            <xsl:value-of select="substring-before(COL5,'2')"/>
          </xsl:when>
          <xsl:when test="substring(COL5,3,1) = '1'">
            <xsl:value-of select="substring-before(COL5,'1')"/>
          </xsl:when>
          <xsl:when test="substring(COL5,3,1) = '2'">
           <xsl:value-of select="substring-before(COL5,'2')"/>
          </xsl:when>
          <xsl:when test="substring(COL5,4,1) = '1'">
            <xsl:value-of select="substring-before(COL5,'1')"/>
          </xsl:when>
          <xsl:when test="substring(COL5,4,1) = '2'">
            <xsl:value-of select="substring-before(COL5,'2')"/>
          </xsl:when>
          <xsl:when test="substring(COL5,5,1) = '1'">
            <xsl:value-of select="substring-before(COL5,'1')"/>
          </xsl:when>
          <xsl:when test="substring(COL5,5,1) = '2'">
            <xsl:value-of select="substring-before(COL5,'2')"/>
          </xsl:when>
    </xsl:choose>
		</xsl:variable>

		<xsl:variable name="varUnderlyingLength">
			<xsl:value-of select="string-length($varRoot)"/>
		</xsl:variable>

		<xsl:variable name="varExYear">
			<xsl:value-of select="substring($varBBCode,($varUnderlyingLength +1),2)"/>
		</xsl:variable>

		<xsl:variable name="varStrike">
			<xsl:value-of select="format-number(substring($varBBCode,($varUnderlyingLength +6)), '#.00')"/>
		</xsl:variable>

		<xsl:variable name="varExDay">
			<xsl:value-of select="substring($varBBCode,($varUnderlyingLength +3),2)"/>
		</xsl:variable>

		<xsl:variable name="varExpiryDay">
			<xsl:choose>
				<xsl:when test="substring($varExDay,1,1)= '0'">
					<xsl:value-of select="substring($varExDay,2,1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$varExDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varMonthCode">
			<xsl:value-of select="substring(COL5,($varUnderlyingLength + 5),1)"/>
		</xsl:variable>

		<xsl:variable name="varExMonth">
			<xsl:call-template name="MonthNo">
				<xsl:with-param name="varMonth" select="$varMonthCode"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name='varThirdFriday'>
			<xsl:value-of select='my:Now(number(concat("20",$varExYear)),number($varExMonth))'/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
				<xsl:value-of select="normalize-space(concat('O:', $varRoot, ' ', $varExYear,$varMonthCode,$varStrike))"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="normalize-space(concat('O:', $varRoot, ' ', $varExYear,$varMonthCode,$varStrike,'D',$varExpiryDay))"/>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>
	
  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">


        <xsl:if test="number(COL4) and (COL35='C' or COL35='P') and ( COL2='24222066' ) ">
			
          <PositionMaster>
			    <xsl:variable name = "PB_COMPANY" >
				    <xsl:value-of select="COL18"/>
			    </xsl:variable>
			    <xsl:variable name="PRANA_SYMBOL">
				    <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='WedbushS']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
			    </xsl:variable>

			  <xsl:variable name = "PB_FUND_NAME" >
					  <xsl:value-of select="COL2"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_FUND_NAME">
				  <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Wedbush']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
			  </xsl:variable>

				  <xsl:variable name="varUnderlying">
					 <xsl:value-of select ="COL36"/>
				  </xsl:variable>

			  <UnderLyingSymbol>
				  <xsl:choose>
					  <xsl:when test ="$varUnderlying!=''">
						  <xsl:value-of select ="$varUnderlying"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </UnderLyingSymbol>

        <xsl:variable name ="varStrikePrice">
				  <xsl:value-of select ="COL38"/>
			  </xsl:variable>
			  <StrikePrice>
				  <xsl:value-of select ="$varStrikePrice"/>
			  </StrikePrice>

			  <Multiplier>
				  <xsl:value-of select ="100"/>
			  </Multiplier>

			  <xsl:variable name ="varPutCall">
				  <xsl:value-of select ="COL35"/>
			  </xsl:variable>

			  <PutOrCall>			
				  <xsl:choose>
					  <xsl:when test ="$varPutCall='P'">
						  <xsl:value-of select ="'0'"/>
					  </xsl:when >
					  <xsl:when test ="$varPutCall='C'">
						  <xsl:value-of select ="'1'"/>
					  </xsl:when >
					  <xsl:otherwise>
						  <xsl:value-of select ="'2'"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </PutOrCall>

			  <ExpirationDate>
				  <xsl:value-of select ="COL37"/>
			  </ExpirationDate>

			  <TickerSymbol>
				  <xsl:choose>
					  <xsl:when test ="$PRANA_SYMBOL != ''">
						  <xsl:value-of select ="$PRANA_SYMBOL"/>
					  </xsl:when>
					  <xsl:when test="COL35 = 'C' or COL35 = 'P'">
						  <xsl:call-template name="ConvertBBCodetoTicker">
							  <xsl:with-param name="varBBCode" select="COL5"/>
						  </xsl:call-template>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="$varUnderlying"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </TickerSymbol>

			  <AUECID>
				  <xsl:value-of select ="12"/>
			  </AUECID>

			  <LongName>
				  <xsl:if test="COL5 != '' and COL5 != '*'">
					  <xsl:value-of select="COL5"/>
				  </xsl:if>
			  </LongName>

			  <PBSymbol>
				  <xsl:if test="COL5 != '' and COL5 != '*'">
					  <xsl:value-of select="COL5"/>
				  </xsl:if>
			  </PBSymbol>

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


