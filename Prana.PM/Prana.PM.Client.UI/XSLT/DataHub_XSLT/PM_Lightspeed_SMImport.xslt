<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

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
	<xsl:template name="varMonthsCode">
		<xsl:param name="varMonth"/>
		<xsl:choose>
			<xsl:when test="$varMonth=1">
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$varMonth=2">
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$varMonth=3">
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$varMonth=4">
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$varMonth=5">
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$varMonth=6">
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$varMonth=7">
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$varMonth=8">
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$varMonth=9">
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$varMonth=10">
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$varMonth=11">
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$varMonth=12">
				<xsl:value-of select="'DCE'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month='JAN'">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month='FEB'">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month='MAR'">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month='APR'">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month='MAY'">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month='JUN'">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month='JUL'">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month='AUG'">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month='SEP'">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month='OCT'">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month='NOV'">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month='DEC'">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='JAN'">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month='FEB'">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month='MAR'">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month='APR'">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month='MAY'">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month='JUN'">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month='JUL'">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month='AUG'">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month='SEP'">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month='OCT'">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month='NOV'">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month='DEC'">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="substring-before(COL18,' ')='C' or substring-before(COL18,' ')='P'">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(substring-after(COL18,' '),' ')"/>
				<!--<xsl:value-of select="normalize-space(COL36)"/>-->
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after(COL37,'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring-before(substring-after(substring-after(COL18,' '),' '),' ')"/>			
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(substring-after(COL37,'/'),'/'),3,2)"/>
			</xsl:variable>
			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring-before(COL18,' ')"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(COL38,'##.00')"/>				
			</xsl:variable>
			<xsl:variable name="MonthCodVar">
				<xsl:call-template name="MonthCode">
					<xsl:with-param name="Month" select="$ExpiryMonth"/>
					<xsl:with-param name="PutOrCall" select="$PutORCall"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="Day">
				<xsl:choose>
					<xsl:when test="substring($ExpiryDay,1,1)='0'">
						<xsl:value-of select="substring($ExpiryDay,2,1)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$ExpiryDay"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="ThirdFriday">
				<xsl:choose>
					<xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
						<xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
					</xsl:when>
				</xsl:choose>
			</xsl:variable>			
			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>
		</xsl:if>
	</xsl:template>

	
	<xsl:template name="spaces">
		<xsl:param name="count"/>
		<xsl:if test="number($count)">
			<xsl:call-template name="spaces">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="' '"/>
	</xsl:template>


	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>
		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
		<xsl:for-each select="//PositionMaster">   

          <PositionMaster>			  
                    <TickerSymbol>
						<xsl:call-template name="Option">
							<xsl:with-param name="Symbol" select="normalize-space(COL36)"/>
							<xsl:with-param name="Suffix" select="''"/>
						</xsl:call-template>
                    </TickerSymbol>

			 

			  <xsl:variable name="UnderlyingSymbol">
				  <xsl:choose>
					  <xsl:when test="string-length(substring-before(substring-after(COL18,' '),' '))='5'">
						  <xsl:value-of select="concat(substring-before(substring-after(COL18,' '),' '),' ')"/>
					  </xsl:when>
					  <xsl:when test="string-length(substring-before(substring-after(COL18,' '),' '))='4'">
						  <xsl:value-of select="concat(substring-before(substring-after(COL18,' '),' '),' ',' ')"/>
					  </xsl:when>
					  <xsl:when test="string-length(substring-before(substring-after(COL18,' '),' '))='3'">
						  <xsl:value-of select="concat(substring-before(substring-after(COL18,' '),' '),' ',' ',' ')"/>
					  </xsl:when>
					  <xsl:when test="string-length(substring-before(substring-after(COL18,' '),' '))='2'">
						  <xsl:value-of select="concat(substring-before(substring-after(COL18,' '),' '),' ',' ',' ',' ')"/>
					  </xsl:when>
					  <xsl:when test="string-length(substring-before(substring-after(COL18,' '),' '))='1'">
						  <xsl:value-of select="concat(substring-before(substring-after(COL18,' '),' '),' ',' ',' ',' ',' ')"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>				 
			  </xsl:variable>
			  <xsl:variable name="ExpiryDay">
				  <xsl:choose>
					  <xsl:when test="string-length(substring-before(substring-after(COL37,'/'),'/'))='2'">
						  <xsl:value-of select="substring-before(substring-after(COL37,'/'),'/')"/>
					  </xsl:when>
					  <xsl:when test="string-length(substring-before(substring-after(COL37,'/'),'/'))='1'">
						  <xsl:value-of select="concat(0,substring-before(substring-after(COL37,'/'),'/'))"/>
					  </xsl:when>
				  </xsl:choose>				  
			  </xsl:variable>
			  <xsl:variable name="ExpiryMonth">
				  <xsl:choose>
					  <xsl:when test="string-length(substring-before(COL37,'/'))='2'">
						  <xsl:value-of select="substring-before(COL37,'/')"/>
					  </xsl:when>
					  <xsl:when test="string-length(substring-before(COL37,'/'))='1'">
						  <xsl:value-of select="concat(0,substring-before(COL37,'/'))"/>
					  </xsl:when>
				  </xsl:choose>
			  </xsl:variable>
			  <xsl:variable name="ExpiryYear">
				  <xsl:value-of select="substring(substring-after(substring-after(COL37,'/'),'/'),3,2)"/>
			  </xsl:variable>
			  
			  <xsl:variable name="PutORCall">
				  <xsl:value-of select="COL35"/>
			  </xsl:variable>


			  <xsl:variable name="VarStrickPrice">
				  <xsl:value-of select="COL38 * 1000"/>
			  </xsl:variable>
			  
			  
			  <xsl:variable name="StrikePrice">
				  <xsl:choose>
					  <xsl:when test="contains(COL38,'.')">
						  <xsl:choose>
							  <xsl:when test="string-length(substring-before(COL38,'.'))='1'">
								  <xsl:value-of select="concat('0000',$VarStrickPrice)"/>
							  </xsl:when>
							  <xsl:when test="string-length(substring-before(COL38,'.'))='2'">
								  <xsl:value-of select="concat('000',$VarStrickPrice)"/>
							  </xsl:when>
							  <xsl:when test="string-length(substring-before(COL38,'.'))='3'">
								  <xsl:value-of select="concat('00',$VarStrickPrice)"/>
							  </xsl:when>
							  <xsl:when test="string-length(substring-before(COL38,'.'))='4'">
								  <xsl:value-of select="concat(0,$VarStrickPrice)"/>
							  </xsl:when>
							  <xsl:when test="string-length(substring-before(COL38,'.'))='5'">
								  <xsl:value-of select="$VarStrickPrice"/>
							  </xsl:when>
						  </xsl:choose>						 
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:choose>
							  <xsl:when test="string-length(COL38)='1'">
								  <xsl:value-of select="concat('0000',$VarStrickPrice)"/>
							  </xsl:when>
							  <xsl:when test="string-length(COL38)='2'">
								  <xsl:value-of select="concat('000',$VarStrickPrice)"/>
							  </xsl:when>
							  <xsl:when test="string-length(COL38)='3'">
								  <xsl:value-of select="concat('00',$VarStrickPrice)"/>
							  </xsl:when>
							  <xsl:when test="string-length(COL38)='4'">
								  <xsl:value-of select="concat(0,$VarStrickPrice)"/>
							  </xsl:when>
							  <xsl:when test="string-length(COL38)='5'">
								  <xsl:value-of select="$VarStrickPrice"/>
							  </xsl:when>
						  </xsl:choose>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>		  
			 
			

			  <xsl:variable name="Symbol">
				  <xsl:value-of select="concat($UnderlyingSymbol,$ExpiryYear,$ExpiryMonth,$ExpiryDay,$PutORCall,$StrikePrice)"/>
			  </xsl:variable>

			  <OSIOptionSymbol>
				  <xsl:choose>
					  <xsl:when test="substring-before(COL18,' ')='C' or substring-before(COL18,' ')='P'">
						  <xsl:value-of select="$Symbol"/>
					  </xsl:when>						  
				  </xsl:choose>				  
			  </OSIOptionSymbol>

			  <IDCOOptionSymbol>
				  <xsl:choose>
					  <xsl:when test="substring-before(COL18,' ')='C' or substring-before(COL18,' ')='P'">
						  <xsl:value-of select="concat($Symbol,'U')"/>
					  </xsl:when>
				  </xsl:choose>
            </IDCOOptionSymbol>

            <UnderLyingSymbol>
              <xsl:value-of select="normalize-space(COL36)"/>
            </UnderLyingSymbol>

            <StrikePrice>
              <xsl:choose>
                <xsl:when test="number(COL38)">
                  <xsl:value-of select="COL38"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </StrikePrice>

            <AUECID>
                 <xsl:value-of select="12"/>
            </AUECID>

            <ExpirationDate>
              <xsl:value-of select="COL37"/>
            </ExpirationDate>

            <AssetCategory>
                  <xsl:value-of select="'Equity Option'"/>
            </AssetCategory>

            <PutOrCall>
              <xsl:choose>
                <xsl:when test="substring-before(COL18,' ')='P'">
                  <xsl:value-of select="0"/>
                </xsl:when>
                <xsl:when test="substring-before(COL18,' ')='C'">
                  <xsl:value-of select="1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="2"/>
                </xsl:otherwise>
              </xsl:choose>
            </PutOrCall>

            <DividendYield>
              <xsl:value-of select ="0"/>
            </DividendYield>

            <Dividend>
              <xsl:value-of select ="0"/>
            </Dividend>

            <DividendAmtRate>
              <xsl:value-of select ="0"/>
            </DividendAmtRate>

            <DivDistributionDate>
              <xsl:value-of select ="'1/1/0001'"/>
            </DivDistributionDate>

            <RequestedSymbology>
              <xsl:value-of select="0"/>
            </RequestedSymbology>

            <Multiplier>
              <xsl:value-of select="100"/>
            </Multiplier>			 
			 
			  <xsl:variable name="varExpiryMonth">
				  <xsl:value-of select="substring-before(COL37,'/')"/>
			  </xsl:variable>
			  <xsl:variable name="varExpiryYear">
				  <xsl:value-of select="substring-after(substring-after(COL37,'/'),'/')"/>
			  </xsl:variable>
			  <xsl:variable name="varPutORCall">
				  <xsl:choose>
					  <xsl:when test="contains(COL35,'P')">
						  <xsl:value-of select="'PUT'"/>
					  </xsl:when>
					  <xsl:when test="contains(COL35,'C')">
						  <xsl:value-of select="'CALL'"/>
					  </xsl:when>
				  </xsl:choose>
				  <xsl:value-of select="COL35"/>
			  </xsl:variable>
			  <xsl:variable name="varStrikePrice">
				  <xsl:value-of select="COL38"/>
			  </xsl:variable>
			  <xsl:variable name="varMonthCodVar">
				  <xsl:call-template name="varMonthsCode">
					  <xsl:with-param name="varMonth" select="$varExpiryMonth"/>					 
				  </xsl:call-template>
			  </xsl:variable>
            <LongName>
				<xsl:choose>
					<xsl:when test="substring-before(COL18,' ')='C' or substring-before(COL18,' ')='P'">
						<xsl:value-of select="concat($UnderlyingSymbol,' ',$varMonthCodVar,' ',$varExpiryYear,' ',$varStrikePrice,' ',$varPutORCall)"/>
					</xsl:when>
				</xsl:choose>				
            </LongName>

            <Symbol_PK>
              <xsl:value-of select ="0"/>
            </Symbol_PK>

            <ReutersSymbol>
              <xsl:value-of select="''"/>
            </ReutersSymbol>

            <BloombergSymbol>
              <xsl:value-of select="''"/>
            </BloombergSymbol>

            <ISINSymbol>
              <xsl:value-of select="''"/>
            </ISINSymbol>

            <SedolSymbol>
              <xsl:value-of select="''"/>
            </SedolSymbol>

            <OPRAOptionSymbol>
              <xsl:value-of select="''"/>
            </OPRAOptionSymbol>

            <CusipSymbol>
              <xsl:value-of select="''"/>
            </CusipSymbol>

            <SymbolType>
              <xsl:value-of select="'0'"/>
            </SymbolType>

            <Delta>
              <xsl:value-of select="'1'"/>
            </Delta>

            <AccrualBasisID>
              <xsl:value-of select="'0'"/>
            </AccrualBasisID>

          </PositionMaster>    
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
