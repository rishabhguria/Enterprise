<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

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

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="varCash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL21"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="(number($varCash))">
		  

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Velocity'"/>
            </xsl:variable>     
          
            <!-- <xsl:variable name="PB_FUND_NAME" select="concat(normalize-space(COL22),'-',normalize-space(COL23),'-',normalize-space(COL24),'-',normalize-space(COL25),'-',normalize-space(COL26))"/> -->
 <xsl:variable name="PB_FUND_NAME">
           <xsl:choose>
				<xsl:when test="COL26='*' or COL26=''">
           <xsl:value-of select="concat(normalize-space(COL22),'-',normalize-space(COL23),'-',normalize-space(COL24),'-',normalize-space(COL25))"/>
           </xsl:when>
           <xsl:when test="COL22='*' or COL22=''">
				<xsl:value-of select="concat(normalize-space(COL23),'-',normalize-space(COL24),'-',normalize-space(COL25),'-',normalize-space(COL26))"/>
           </xsl:when>
		   <xsl:when test="COL23='*' or COL23=''">
				<xsl:value-of select="concat(normalize-space(COL22),'-',normalize-space(COL24),'-',normalize-space(COL25),'-',normalize-space(COL26))"/>
           </xsl:when>
		   <xsl:when test="COL24='*' or COL24=''">
				<xsl:value-of select="concat(normalize-space(COL22),'-',normalize-space(COL23),'-',normalize-space(COL25),'-',normalize-space(COL26))"/>
           </xsl:when>
		     <xsl:when test="COL25='*' or COL25=''">
				<xsl:value-of select="concat(normalize-space(COL22),'-',normalize-space(COL23),'-',normalize-space(COL24),'-',normalize-space(COL26))"/>
           </xsl:when>
           <xsl:otherwise>
				<xsl:value-of select="concat(normalize-space(COL22),'-',normalize-space(COL23),'-',normalize-space(COL24),'-',normalize-space(COL25),'-',normalize-space(COL26))"/>
           </xsl:otherwise>
           </xsl:choose>
        </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
			  
            <AccountName>
              <xsl:choose>
                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>             
            </AccountName>

            <xsl:variable name="varCurrencyName">
              <xsl:value-of select="'USD'"/>
            </xsl:variable>

            <CurrencyName>
              <xsl:value-of select="$varCurrencyName"/>
            </CurrencyName>

            <xsl:variable name = "varAmount" >
              <xsl:choose>
                <xsl:when test="$varCash &gt; 0">
                  <xsl:value-of select="$varCash"/>
                </xsl:when>
                <xsl:when test="$varCash &lt; 0">
                  <xsl:value-of select="$varCash*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>        

            <JournalEntries>
              <xsl:choose>
                <xsl:when test="$varCash &lt; 0">
                  <xsl:value-of select="concat('Cash:', $varAmount , '|', 'Intincome', ':' , $varAmount)"/>
                </xsl:when>
                <xsl:when  test="$varCash &gt; 0">
                  <xsl:value-of select="concat('IntExpense',':' , $varAmount , '|Cash:' , $varAmount)"/>
                </xsl:when>
              </xsl:choose>
            </JournalEntries>
           
		    <Date>
		     <xsl:value-of select="COL5"/>
		    </Date>				  		
           
            <Description>
              <xsl:value-of select="COL37"/>
            </Description>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>